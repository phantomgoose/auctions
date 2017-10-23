using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using netbelt.ViewModels;
using netbelt.Models;
using netbelt.Contexts;
using Microsoft.EntityFrameworkCore;

namespace netbelt.Controllers
{
    public class AuctionController : Controller
    {

        private readonly NetBeltContext _context;

        public AuctionController(NetBeltContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("auctions")]
        public IActionResult Index()
        {
            // bootleg route protection
            if (!isLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }
            User user = _context.Users.Find(HttpContext.Session.GetInt32("UserID"));
            if (user != null) {
                // THIS DOES NOT INCLUDE HOLDS FROM BIDS. Intentional.
                ViewBag.Wallet = user.WalletBalance;
            }
            return View();
        }

        [HttpGet]
        [Route("auctions/new")]
        public IActionResult CreateForm()
        {
            if (!isLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [Route("auctions")]
        public IActionResult Create(CreateAuctionViewModel model)
        {
            if (!isLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                Auction auction = new Auction {
                    ProductName = model.ProductName,
                    Description = model.Description,
                    StartingBid = model.StartingBid,
                    EndDate = model.EndDate,
                    UserID = (int)HttpContext.Session.GetInt32("UserID"), // safe, since isLoggedIn checks for ID's existence
                };
                _context.Auctions.Add(auction);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("CreateForm");
        }

        [HttpGet]
        [Route("auction/{id}")]
        public IActionResult ShowAuction(int id) {
            if (!isLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }
            Auction auction = _context.Auctions.Include(a => a.User).Include(a => a.Bids).ThenInclude(b => b.User).Where(a => a.ID == id).SingleOrDefault();
            if (auction != null) {
                ViewBag.Now = DateTime.UtcNow;
                return View(auction);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("auction/{id}/delete")] // this should be a delete http request, but it would require JS on the client side which im trying to avoid
        public IActionResult DeleteAuction(int id) {
            if (!isLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }
            Auction auction = _context.Auctions.Find(id);
            // make sure user really does own the auction
            if (auction.UserID == HttpContext.Session.GetInt32("UserID")) {
                _context.Auctions.Remove(auction);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("bids")]
        public IActionResult CreateBid(BidViewModel model) {
            if (!isLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid) {
                Bid bid = new Bid {
                    Amount = model.Amount,
                    UserID = model.UserID,
                    AuctionID = model.AuctionID,
                };
                _context.Bids.Add(bid);
                _context.SaveChanges();
                return RedirectToAction("ShowAuction", new {id = model.AuctionID});
            }
            Auction auction = _context.Auctions.Include(a => a.User).Include(a => a.Bids).ThenInclude(b => b.User).Where(a => a.ID == model.AuctionID).SingleOrDefault();
            ViewBag.Now = DateTime.UtcNow;
            return View("ShowAuction", auction);
        }

        // bootleg.jpg
        private bool isLoggedIn()
        {
            return HttpContext.Session.Keys.Contains("UserID") && HttpContext.Session.GetInt32("UserID") != null;
        }
    }
}