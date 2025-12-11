namespace DTOs
{
    public record UserDTO(
     int Id ,

     string Email ,

     string FirstName ,

     string LastName ,

     string Password,

     ICollection<OrderDTO> Orders 
    );
    

    
}
