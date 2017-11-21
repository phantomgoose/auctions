using System;
using System.Collections.Generic;

namespace Auctions.Models {
    public class Auction : BaseEntity {
        public string ProductName {get; set;}
        public string Description {get; set;}
        public DateTime EndDate {get; set;}
        public double StartingBid {get; set;}
        public int UserID {get; set;}
        public User User {get; set;}
        public List<Bid> Bids {get; set;} = new List<Bid>();
        public bool Resolved {get; set;} = false;
    }
}