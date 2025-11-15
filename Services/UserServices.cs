using Entities;
using Repositories;
using System.Security.Cryptography.X509Certificates;

namespace Services
{
    public class UserServices : IUserServices
    {
        private IUserRepositories _userRepository;
        private IPasswordServices _passwordService;

        public UserServices(IUserRepositories userRepository, IPasswordServices passwordService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
        }
        public IEnumerable<User> GetUser()
        {
            return _userRepository.GetUsers();
        }
        public User GetUsersById(int id)
        {
            return _userRepository.GetUserByID(id);
        }
        public User AddUser(User user)
        {
            int passScore = _passwordService.GetPasswordScore(user.Password);
            if (passScore < 2)
                return null;
            return _userRepository.AddUser(user);
        }
        public bool UpdateUser(int id, User user)
        {
            int passScore = _passwordService.GetPasswordScore(user.Password);
            if (passScore < 2)
                return false;
            _userRepository.UpdateUser(id, user);
            return true;
        }
        public User Login(LoginUser loginUser)
        {
            return _userRepository.Login(loginUser);
        }

    }
}
