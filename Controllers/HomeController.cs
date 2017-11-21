using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Auctions.Models;
using Auctions.ViewModels;
using Auctions.Contexts;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;

namespace Auctions.Controllers
{
    public class HomeController : Controller
    {
        private readonly AuctionsContext _context;

        public HomeController (AuctionsContext context) {
            _context = context;
        }

        // displays login/registration forms
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            processAuctions();
            return View();
        }

        // logs a user in
        [HttpPost]
        [Route("login")]
        public IActionResult Login(LoginViewModel model) {
            if (ModelState.IsValid) {
                User user = _context.Users.SingleOrDefault(u => u.Username == model.LoginUsername);
                // this validation should probably live in extensions as a custom validation attribute for the login VM
                if (user != null && BCrypt.Net.BCrypt.Verify(model.LoginPassword, user.Password)) {
                    HttpContext.Session.SetInt32("UserID", user.ID);
                    HttpContext.Session.SetString("FirstName", user.FirstName);
                    return RedirectToAction("Index", "Auction");
                }
            }
            ModelState.AddModelError("LoginUsername", "Invalid username or password");
            return View("Index");
        }

        // registers a user
        [HttpPost]
        [Route("register")]
        public IActionResult Register(RegistrationViewModel model) {
            if (ModelState.IsValid) {
                // make sure a user with specified username doesn't already exist (this should probably live in Extensions as a custom validation attribute for the registration VM)
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

        // clears session, thus barring access to protected routes
        [HttpGet]
        [Route("logout")]
        public IActionResult Logout() {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        // goes through the list of auctions that have ended, but haven't been resolved yet, and settles the transactions. Runs on home page refresh. Probably should run asynchronously at an interval on the server instead. Bootleg.png
        private void processAuctions() {
            List<Auction> ended_auctions = _context.Auctions.Include(a => a.Bids).Include(a => a.User).Where(a => a.Resolved == false && a.EndDate <= DateTime.UtcNow).ToList();
            foreach(var auction in ended_auctions) {
                var auction_owner = auction.User;
                var top_bid = auction.Bids.Where(b => b.Highest).SingleOrDefault();
                if (top_bid != null) {
                    var bidding_user = _context.Users.Find(top_bid.UserID);
                    // once we have a user that created the auction and the user who made the top bid, transfer money from one to the other.
                    auction_owner.WalletBalance += top_bid.Amount;
                    bidding_user.WalletBalance -= top_bid.Amount;
                    // unflag bid as Highest to remove the hold. Come to think of it, Highest was probably not the best variable name ever. Come to think of it, this isn't the greatest way to handle payment contracts to begin with.
                    top_bid.Highest = false;
                    auction.Resolved = true;
                }
            }
            _context.SaveChanges();
        }
    }
}
