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
            if (await _userRepository.IsEmailExists(user.Email)!=null)
            {
                return null;
            }

            int passScore = _passwordService.GetPasswordScore(user.Password);
            if (passScore < MinimumPasswordScore)
                return null;
            return _mapper.Map<User,PostUserDTO >(await _userRepository.AddUser(_mapper.Map<PostUserDTO,User>(user)));
        }

        public async Task<bool> UpdateUser(int id, PostUserDTO user)
        {
            User userWithSameEmail = await _userRepository.IsEmailExists(user.Email);
            if (userWithSameEmail != null && userWithSameEmail.Id != user.Id)
                return false;
            int passScore = _passwordService.GetPasswordScore(user.Password);
            if (passScore < MinimumPasswordScore)
                return false;
            await _userRepository.UpdateUser(id, _mapper.Map<PostUserDTO, User>(user));
            return true;
        }
        
        public async Task<UserDTO> Login(LoginUserDTO loginUser)
        {
            return _mapper.Map < User, UserDTO> (await _userRepository.Login(_mapper.Map<LoginUserDTO, User>(loginUser)));
        }
    }
}
