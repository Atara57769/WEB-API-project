
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class LoginUser
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
