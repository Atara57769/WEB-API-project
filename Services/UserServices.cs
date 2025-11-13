using Entities;
using Repositories;
using System.Security.Cryptography.X509Certificates;

namespace Services
{
    public class UserServices
    {
        private UserRepositories userRepository = new UserRepositories();
        private PasswordServices passwordService = new PasswordServices();
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
            int passScore = passwordService.GetPasswordScore(user.Password);
            if (passScore < 2)
                return null;
            return userRepository.AddUser(user);
        }
        public bool UpdateUser(int id,User user)
        {
            int passScore = passwordService.GetPasswordScore(user.Password);
            if (passScore < 2)
                return false;
            userRepository.UpdateUser(id,user);
            return true;
        }
        public User Login(LoginUser loginUser)
        {
            return userRepository.Login(loginUser);
        }

    }
}
