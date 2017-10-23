using System.ComponentModel.DataAnnotations;

namespace netbelt.ViewModels
{
    public class RegistrationViewModel
    {
        private const string FirstNameValidationMessage = "First name is required and must be at least two characters long.";
        private const string LastNameValidationMessage = "Last name is required and must be at least two characters long.";
        private const string PasswordValidationMessage = "Password must be at least 8 characters long.";
        private const string CPasswordValidationMessage = "Password confirmation is required and must match your password.";
        private const string UsernameValidationMessage = "Username must be between 3 and 20 characters long (exclusive).";

        // no lenght reqs specified for name
        [Required(ErrorMessage = FirstNameValidationMessage)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = LastNameValidationMessage)]
        public string LastName { get; set; }

        // assignment requirement is worded as follows: Username must be greater than 3 but less than 20. So from 4 to 19 inclusive.
        [Required(ErrorMessage = UsernameValidationMessage)]
        [MinLength(4, ErrorMessage = UsernameValidationMessage)]
        [MaxLength(19, ErrorMessage = UsernameValidationMessage)]
        public string RegistrationUsername {get; set;}

        [Required(ErrorMessage = PasswordValidationMessage)]
        [MinLength(8, ErrorMessage = PasswordValidationMessage)]
        public string RegistrationPassword { get; set; }

        [Required(ErrorMessage = CPasswordValidationMessage)]
        [Compare("RegistrationPassword", ErrorMessage = CPasswordValidationMessage)]
        public string PasswordConfirmation { get; set; }
    }
}