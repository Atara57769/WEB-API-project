using Entities;

namespace Services
{
    public interface IUserServices
    {
        Task<User> AddUser(User user);
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUserById(int id);
        Task<User> Login(LoginUser loginUser);
        Task<bool> UpdateUser(int id, User user);
    }
}
