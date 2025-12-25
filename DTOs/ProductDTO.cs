using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
     public record ProductDTO(
     int Id ,

     int CategoryId ,

     string ProductName ,

     string Description ,

     double Price ,

     string ImageUrl ,

     string CategoryName 
        );
  
}
