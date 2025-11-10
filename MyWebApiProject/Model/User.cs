using System.ComponentModel.DataAnnotations;

namespace MyProject.Model
{
    public class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int UserId { get; set; }
        
    }
}
