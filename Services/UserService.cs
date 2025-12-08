using Entities;
using Repositories;

namespace Services
{
    public class UserServices : IUserService
    {
        private const int MinimumPasswordScore = 2;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;

        public UserServices(IUserRepository userRepository, IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _userRepository.GetUsers();
        }

        public async Task<User> GetUserById(int id)
        {
            return await _userRepository.GetUserById(id);
        }

        public async Task<User> AddUser(User user)
        {
            if (await _userRepository.IsEmailExists(user.Email))
            {
                return null;
            }

            int passScore = _passwordService.GetPasswordScore(user.Password);
            if (passScore < MinimumPasswordScore)
                return null;
            return await _userRepository.AddUser(user);
        }

        public async Task<bool> UpdateUser(int id, User user)
        {
            int passScore = _passwordService.GetPasswordScore(user.Password);
            if (passScore < MinimumPasswordScore)
                return false;
            await _userRepository.UpdateUser(id, user);
            return true;
        }
        
        public Task<User> Login(LoginUser loginUser)
        {
            return _userRepository.Login(loginUser);
        }
    }
}
