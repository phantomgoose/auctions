using System;
using System.ComponentModel.DataAnnotations;

namespace netbelt.Extensions {
    // verifies that the auction end date is in the future
    public class EnsureValidAuctionEndDateAttribute : ValidationAttribute {
        protected override ValidationResult IsValid(object value, ValidationContext context) {
            // potentially unsafe since datetime is not nullable
            // additionally, without time zone conversion this can result in validation errors depending on where the client is located/what locale their browser is using/what browser they're using. As a matter of fact, this will probably break.
            if (DateTime.UtcNow >= (DateTime)value) {
                return new ValidationResult(ErrorMessage);
            }
            return ValidationResult.Success;
        }
    }
}