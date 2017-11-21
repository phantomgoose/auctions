using System;
using System.ComponentModel.DataAnnotations;
using Auctions.Extensions;

namespace Auctions.ViewModels
{
    public class CreateAuctionViewModel
    {
        private const string ProductNameValidationError = "Product name must be at least 4 characters long.";
        private const string DescriptionValidationError = "Description must be at least 11 characters long.";
        private const string StartingBidValidationError = "Starting bid must be greater than zero.";
        private const string EndDateValidationError = "End date must be in the future.";

        [Required(ErrorMessage = ProductNameValidationError)]
        [MinLength(4, ErrorMessage = ProductNameValidationError)]
        public string ProductName { get; set; }

        [Required(ErrorMessage = DescriptionValidationError)]
        [MinLength(11, ErrorMessage = DescriptionValidationError)]
        public string Description { get; set; }

        [Required(ErrorMessage = StartingBidValidationError)]
        [Range(.01, double.MaxValue, ErrorMessage = StartingBidValidationError)]
        public double StartingBid {get; set;}

        [Required(ErrorMessage = EndDateValidationError)]
        [EnsureValidAuctionEndDate(ErrorMessage = EndDateValidationError)]
        public DateTime EndDate {get; set;}
    }
}