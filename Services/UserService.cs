using AutoMapper;
using DTOs;
using Entities;
using Repositories;

namespace Services
{
    public class UserServices : IUserService
    {
        private const int MinimumPasswordScore = 2;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IMapper _mapper;


        public UserServices(IUserRepository userRepository, IPasswordService passwordService, IMapper mapper)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDTO>> GetUsers()
        {
             return _mapper.Map<IEnumerable<User>, IEnumerable<UserDTO>>(await _userRepository.GetUsers());
        }

        public async Task<UserDTO> GetUserById(int id)
        {
             return _mapper.Map<User, UserDTO>(await _userRepository.GetUserById(id));
        }

        public async Task<PostUserDTO> AddUser(PostUserDTO user)
        {
            return _mapper.Map<User,PostUserDTO >(await _userRepository.AddUser(_mapper.Map<PostUserDTO,User>(user)));
        }

        public async Task UpdateUser(int id, PostUserDTO user)
        {
            await _userRepository.UpdateUser(id, _mapper.Map<PostUserDTO, User>(user));
        }
        
        public async Task<UserDTO> Login(LoginUserDTO loginUser)
        {
            return _mapper.Map < User, UserDTO> (await _userRepository.Login(loginUser.Email, loginUser.Password));
        }
        public async Task<User> UserWithSameEmail(string email)
        {
            return await _userRepository.UserWithSameEmail(email);
        }
        public bool IsPasswordStrong(string password)
        {
            int passScore = _passwordService.GetPasswordScore(password);
            if (passScore < MinimumPasswordScore)
                return false;
            return true;
        }
    }
}
