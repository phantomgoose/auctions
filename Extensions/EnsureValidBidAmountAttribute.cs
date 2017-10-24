using System;
using System.ComponentModel.DataAnnotations;
using netbelt.ViewModels;
using netbelt.Contexts;
using netbelt.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace netbelt.Extensions {
    // verifies the Bid model to ensure the bid amount (and the bid as a whole) is valid
    [AttributeUsage(AttributeTargets.Class)]
    public class EnsureValidBidAmountAttribute : ValidationAttribute {
        protected override ValidationResult IsValid(object value, ValidationContext context) {
            // since this attribute targets the entire model, Value will be the model. Cast it as such.
            var model = (BidViewModel)value;
            // get database context from the injected ValidationContext
            var _context = (NetBeltContext)context.GetService(typeof(NetBeltContext));
            // get target auction
            var auction = _context.Auctions.Include(a => a.Bids).Include(a => a.User).Where(a => a.ID == model.AuctionID).SingleOrDefault();
            // get current top bid
            var current_top_bid = auction.Bids.OrderByDescending(b => b.Amount).Take(1).SingleOrDefault();
            // get user that is trying to add the bid
            var user = _context.Users.Include(u => u.Bids).Where(u => u.ID == model.UserID).SingleOrDefault();
            // if we can't find the user or the auction, something is borked (or someone is messing around client-side)
            if (user == null || auction == null) {
                return new ValidationResult("Invalid user or auction ID");
            }
            // if the auction was created by our user, deny their attempt to bid on their own auction
            if (auction.UserID == model.UserID) {
                return new ValidationResult("You can't bid on your own auction.");
            }
            // if the user is already the top bidding user for this auction, deny the bid
            if (current_top_bid != null && current_top_bid.UserID == model.UserID) {
                return new ValidationResult("You're already the highest bidder!");
            }
            // let's also make sure the auction hasn't expired
            if (DateTime.UtcNow >= auction.EndDate) {
                return new ValidationResult("This auction has expired. You can't submit bids on it anymore.");
            }
            // and that the user has enough unheld wallet funds
            if (model.Amount > user.WalletBalance - user.HeldAmount) {
                return new ValidationResult("You don't have enough money for this bid.");
            }
            // and isn't trying to undercut the competition
            if (current_top_bid != null && model.Amount <= current_top_bid.Amount) {
                return new ValidationResult("Your bid is too low!");
            }
            // and has met the starting bid requirement
            if (model.Amount < auction.StartingBid) {
                return new ValidationResult("Your bid must meet the starting minimum.");
            }
            // if everything is cool, return success
            return ValidationResult.Success;
        }
    }
}