using Entities;

namespace Repositories
{
    public interface IUserRepository
    {
        Task<User> AddUser(User newUser);
        Task<User> GetUserById(int id);
        Task<IEnumerable<User>> GetUsers();
        Task<User> Login(User loginUser);
        Task UpdateUser(int id, User updateUser);
        Task<User> IsEmailExists(string email);
    }
}
