using System.ComponentModel.DataAnnotations;
using netbelt.Extensions;

namespace netbelt.ViewModels {
    // custom validation for bid amount
    [EnsureValidBidAmount]
    public class BidViewModel {
        private const string AmountValidationError = "Bid must be higher than previous highest bid and lower than your available wallet balance!";

        [Required(ErrorMessage = AmountValidationError)]
        public double Amount {get; set;}

        [Required]
        public int UserID {get; set;}

        [Required]
        public int AuctionID {get; set;}
    }
}