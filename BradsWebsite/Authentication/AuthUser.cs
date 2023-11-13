using System.Security.Cryptography;
using System.Text;

namespace BradsWebsite.Authentication
{
    public struct AuthUser
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime? Locked { get; set; }
        public DateTime? Disabled { get; set; }
        public DateTime? Confirmed { get; set; }
        public string AccountType { get; set; }
        public string PasswordHashed()
        {
            var inputBytes = Encoding.UTF8.GetBytes(Password);
            var inputHash = SHA256.HashData(inputBytes);
            return Convert.ToHexString(inputHash);
        }
    }
}
