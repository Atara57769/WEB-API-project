using DTOs;
using Entities;

namespace Services
{
    public interface IUserService
    {
        Task<PostUserDTO> AddUser(PostUserDTO user);
        Task<IEnumerable<UserDTO>> GetUsers();
        Task<UserDTO> GetUserById(int id);
        Task<UserDTO> Login(LoginUserDTO loginUser);
        Task UpdateUser(int id, PostUserDTO user);
        Task<User> UserWithSameEmail(string email);
        public bool IsPasswordStrong(string password);
    }
}
