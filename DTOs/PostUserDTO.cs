using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public record PostUserDTO(
     int Id,
     [EmailAddress]
     [Required]
     string Email,

     string FirstName ,

     string LastName ,

     [Required]
     string Password

    );

}
