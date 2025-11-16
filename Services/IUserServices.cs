using Entities;

namespace Services
{
    public interface IUserServices
    {
        User AddUser(User user);
        IEnumerable<User> GetUsers();
        User GetUserById(int id);
        User Login(LoginUser loginUser);
        bool UpdateUser(int id, User user);
    }
}
