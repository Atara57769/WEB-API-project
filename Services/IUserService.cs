using DTOs;
using Entities;

namespace Services
{
    public interface IUserService
    {
        Task<AuthResultDTO> AddUser(PostUserDTO user);
        Task<IEnumerable<UserDTO>> GetUsers();
        Task<UserDTO> GetUserById(int id);
        Task<AuthResultDTO> Login(LoginUserDTO loginUser);
        Task UpdateUser(int id, PostUserDTO user);
        Task<bool> UserWithSameEmail(string email,int id=-1);
        public bool IsPasswordStrong(string password);
    }
}
