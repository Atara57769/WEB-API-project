using Entities;
using Repositories;

namespace Services
{
    public class UserServices : IUserServices
    {
        private const int MinimumPasswordScore = 2;
        private readonly IUserRepositories _userRepository;
        private readonly IPasswordServices _passwordService;

        public UserServices(IUserRepositories userRepository, IPasswordServices passwordService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
        }

        public IEnumerable<User> GetUsers()
        {
            return _userRepository.GetUsers();
        }

        public User GetUserById(int id)
        {
            return _userRepository.GetUserById(id);
        }

        public User AddUser(User user)
        {
            int passScore = _passwordService.GetPasswordScore(user.Password);
            if (passScore < MinimumPasswordScore)
                return null;
            return _userRepository.AddUser(user);
        }

        public bool UpdateUser(int id, User user)
        {
            int passScore = _passwordService.GetPasswordScore(user.Password);
            if (passScore < MinimumPasswordScore)
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
