namespace netbelt.Models {
    public class Bid : BaseEntity {
        public double Amount {get; set;}
        public int UserID {get; set;}
        public User User {get; set;}
        public int AuctionID {get; set;}
        public Auction Auction {get; set;}
    }
}