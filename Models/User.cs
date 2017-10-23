using System.Collections.Generic;

namespace netbelt.Models {
    public class User : BaseEntity {
        public string Username {get; set;}
        public string Password {get; set;}
        public string FirstName {get; set;}
        public string LastName {get; set;}
        // no currency support for ease of use
        public double WalletBalance {get; set;}
        public List<Auction> Auctions {get; set;} = new List<Auction>();
        public List<Bid> Bids {get; set;} = new List<Bid>();
    }
}