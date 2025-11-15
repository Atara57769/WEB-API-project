using Entities;

namespace Repositories
{
    public interface IUserRepositories
    {
        User AddUser(User newUser);
        User GetUserByID(int id);
        IEnumerable<User> GetUsers();
        User Login(LoginUser loginUser);
        User LoginUser(User loginUser);
        void UpdateUser(int id, User updateUser);
    }
}