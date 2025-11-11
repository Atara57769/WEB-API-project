using Entities;
using Repositories;
using System.Security.Cryptography.X509Certificates;

namespace Services
{
    public class UserServices
    {
        private UserRepositories userRepository = new UserRepositories();

        public  IEnumerable<User> GetUser()
        {
            return userRepository.GetUsers();
        }
        public  User GetUsersById(int id)
        {
           return userRepository.GetUserByID(id);
        }
        public  User AddUser(User user)
        {
            return userRepository.AddUser(user);
        }
        public void UpdateUser(int id,User user)
        {
            userRepository.UpdateUser(id,user);
        }
        public User Login(LoginUser loginUser)
        {
            return userRepository.Login(loginUser);
        }

    }
}
