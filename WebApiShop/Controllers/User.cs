using System.ComponentModel.DataAnnotations;

namespace WebApiShop.Controllers
{
    public class User
    {
        public int Id { get; set; }
       [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}