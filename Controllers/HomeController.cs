using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using netbelt.Models;
using netbelt.ViewModels;
using netbelt.Contexts;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;

namespace netbelt.Controllers
{
    public class HomeController : Controller
    {
        private readonly NetBeltContext _context;

        public HomeController (NetBeltContext context) {
            _context = context;
        }

        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            processAuctions();
            return View();
        }

        // i apologize for routing ugliness. Will fix if there's time
        [HttpPost]
        [Route("login")]
        public IActionResult Login(LoginViewModel model) {
            if (ModelState.IsValid) {
                User user = _context.Users.SingleOrDefault(u => u.Username == model.LoginUsername);
                if (user != null && BCrypt.Net.BCrypt.Verify(model.LoginPassword, user.Password)) {
                    HttpContext.Session.SetInt32("UserID", user.ID);
                    HttpContext.Session.SetString("FirstName", user.FirstName);
                    return RedirectToAction("Index", "Auction");
                }
            }
            ModelState.AddModelError("LoginUsername", "Invalid username or password");
            return View("Index");
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(RegistrationViewModel model) {
            if (ModelState.IsValid) {
                // make sure a user with specified username doesn't already exist
                // all of this logic (and login logic too) should technically live in a service, but I probably won't have time to add it
                if (_context.Users.SingleOrDefault(u => u.Username == model.RegistrationUsername) == null) {
                    User user = new User {
                        Username = model.RegistrationUsername,
                        Password = BCrypt.Net.BCrypt.HashPassword(model.RegistrationPassword),
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        WalletBalance = 1000.00,
                    };
                    _context.Users.Add(user);
                    _context.SaveChanges();
                    HttpContext.Session.SetInt32("UserID", user.ID);
                    HttpContext.Session.SetString("FirstName", user.FirstName);
                    return RedirectToAction("Index", "Auction");
                } else {
                    ModelState.AddModelError("RegistrationUsername", "A user with this username already exists.");
                }
            }
            return View("Index");
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout() {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        private void processAuctions() {
            // i mean aside from creating an async server side function that runs on an interval to check for ended auctions, this is the best solution i can come up with at this time
            List<Auction> ended_auctions = _context.Auctions.Include(a => a.Bids).Include(a => a.User).Where(a => a.EndDate <= DateTime.UtcNow && a.Resolved == false).ToList();
            foreach(var auction in ended_auctions) {
                var auction_owner = auction.User;
                var top_bid = auction.Bids.OrderByDescending(b => b.Amount).Take(1).SingleOrDefault();
                if (top_bid != null) {
                    var bidding_user = _context.Users.Find(top_bid.UserID);
                    auction_owner.WalletBalance += top_bid.Amount;
                    bidding_user.WalletBalance -= top_bid.Amount;
                    auction.Resolved = true;
                }
            }
            _context.SaveChanges();
        }
    }
}
