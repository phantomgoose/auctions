using System;
using System.ComponentModel.DataAnnotations;
using netbelt.Extensions;

namespace netbelt.ViewModels
{
    public class CreateAuctionViewModel
    {
        // odd length requirements here too
        private const string ProductNameValidationError = "Product name must be at least 4 characters long.";
        private const string DescriptionValidationError = "Description must be at least 11 characters long.";
        private const string StartingBidValidationError = "Starting bid must be greater than zero.";
        private const string EndDateValidationError = "End date must be in the future.";

        // not sure if validations are required for login (I think they shouldn't be), adding just in case since test reqs are ambiguous
        [Required(ErrorMessage = ProductNameValidationError)]
        [MinLength(4, ErrorMessage = ProductNameValidationError)]
        public string ProductName { get; set; }

        [Required(ErrorMessage = DescriptionValidationError)]
        [MinLength(11, ErrorMessage = DescriptionValidationError)]
        public string Description { get; set; }

        // could write a custom validator here for the smallest possible double value, but again... w/e. Not line you can bid less than a cent on most platforms anyway
        [Required(ErrorMessage = StartingBidValidationError)]
        [Range(.01, double.MaxValue, ErrorMessage = StartingBidValidationError)]
        public double StartingBid {get; set;}

        [Required(ErrorMessage = EndDateValidationError)]
        [EnsureValidAuctionEndDate(ErrorMessage = EndDateValidationError)]
        public DateTime EndDate {get; set;}
    }
}