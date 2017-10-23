using System.ComponentModel.DataAnnotations;

namespace netbelt.ViewModels
{
    public class LoginViewModel
    {
        private const string UsernameValidationError = "Username is required and must be between 3 and 20 characters long (exclusive).";

        // not sure if validations are required for login (I think they shouldn't be), adding just in case since test reqs are ambiguous
        [Required(ErrorMessage = UsernameValidationError)]
        [MinLength(4, ErrorMessage = UsernameValidationError)]
        [MaxLength(19, ErrorMessage = UsernameValidationError)]
        public string LoginUsername { get; set; }
        public string LoginPassword { get; set; }
    }
}