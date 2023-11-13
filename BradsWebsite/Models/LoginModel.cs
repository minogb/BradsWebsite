using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BradsWebsite.Models
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [PasswordPropertyText(true)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
