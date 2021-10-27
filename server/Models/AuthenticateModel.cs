using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class AuthenticateModel
    {
        public AuthenticateModel(string EmailTest, string PasswordTest)
        {
            Email = EmailTest;
            Password = PasswordTest;
        }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}