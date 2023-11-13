using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BradsWebsite.Models
{
    public class SignUpModel
    {
        [Required]
        [EmailAddress]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "Emails must be between 6 and 255 characters")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 5, ErrorMessage = "User Names must be between 5 and 255 characters")]
        public string Name { get; set; }
        [Required]
        [PasswordPropertyText(true)]
        [DataType(DataType.Password)]
        [StringLength(255, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 255 characters")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password")]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
