using Entities;

namespace Repositories
{
    public interface IUserRepositories
    {
        Task<User> AddUser(User newUser);
        Task<User> GetUserById(int id);
        Task<IEnumerable<User>> GetUsers();
        Task<User> Login(LoginUser loginUser);
        Task UpdateUser(int id, User updateUser);
    }
}
