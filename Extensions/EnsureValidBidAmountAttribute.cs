using System;
using System.ComponentModel.DataAnnotations;
using netbelt.ViewModels;
using netbelt.Contexts;
using netbelt.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace netbelt.Extensions {
    [AttributeUsage(AttributeTargets.Class)]
    public class EnsureValidBidAmountAttribute : ValidationAttribute {
        protected override ValidationResult IsValid(object value, ValidationContext context) {
            var model = (BidViewModel)value;
            var _context = (NetBeltContext)context.GetService(typeof(NetBeltContext));
            var auction = _context.Auctions.Include(a => a.Bids).Include(a => a.User).Where(a => a.ID == model.AuctionID).SingleOrDefault();
            var current_top_bid = auction.Bids.OrderByDescending(b => b.Amount).Take(1).SingleOrDefault();
            var user = _context.Users.Include(u => u.Bids).Where(u => u.ID == model.UserID).SingleOrDefault();
            if (user == null || auction == null) {
                return new ValidationResult("Invalid user or auction ID");
            }
            // let's also make sure the auction hasn't expired
            if (DateTime.UtcNow >= auction.EndDate) {
                return new ValidationResult("This auction has expired. You can't submit bids on it anymore.");
            }
            double held_amount = 0;
            foreach(var bid in user.Bids) {
                held_amount += bid.Amount;
            }
            if (model.Amount > user.WalletBalance - held_amount) {
                return new ValidationResult("You don't have enough money for this bid.");
            }
            if (current_top_bid != null && model.Amount < current_top_bid.Amount) {
                return new ValidationResult("Your bid is too low!");
            }
            return ValidationResult.Success;
        }
    }
}