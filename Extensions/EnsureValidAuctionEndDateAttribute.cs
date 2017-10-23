using System;
using System.ComponentModel.DataAnnotations;

namespace netbelt.Extensions {
    public class EnsureValidAuctionEndDateAttribute : ValidationAttribute {
        protected override ValidationResult IsValid(object value, ValidationContext context) {
            // potentially unsafe since datetime is not nullable
            // additionally, without time zone conversion this can result in validation errors depending on where the client is located
            if (DateTime.UtcNow >= (DateTime)value) {
                return new ValidationResult(ErrorMessage);
            }
            return ValidationResult.Success;
        }
    }
}