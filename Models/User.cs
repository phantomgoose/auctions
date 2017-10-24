using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace netbelt.Models
{
    public class User : BaseEntity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        // no currency support for ease of use
        public double WalletBalance { get; set; }
        public List<Auction> Auctions { get; set; } = new List<Auction>();
        public List<Bid> Bids { get; set; } = new List<Bid>();

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public double HeldAmount
        {
            get
            {
                double? held_amount = this.Bids.Where(b => b.Highest == true).Sum(b => b.Amount);
                return held_amount != null ? (double)held_amount : 0;
            }
            private set { }
        }
    }
}