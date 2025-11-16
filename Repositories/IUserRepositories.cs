using Entities;

namespace Repositories
{
    public interface IUserRepositories
    {
        User AddUser(User newUser);
        User GetUserById(int id);
        IEnumerable<User> GetUsers();
        User Login(LoginUser loginUser);
        void UpdateUser(int id, User updateUser);
    }
}
