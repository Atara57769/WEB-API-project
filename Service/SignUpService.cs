using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;
using Entity;
using Zxcvbn;


namespace Service
{
    public class SignUpService : ISignUpService
    {
        private readonly ISignUpRepository _repo;

        public SignUpService(ISignUpRepository repo)
        {
            _repo = repo;
        }
        public async Task<User?> SignUp(User user)
        {
            if (!string.IsNullOrWhiteSpace(user.FirstName) && 
                !string.IsNullOrWhiteSpace(user.LastName) && 
                !string.IsNullOrWhiteSpace(user.Password) && 
                !string.IsNullOrWhiteSpace(user.UserName))
            {
                return await _repo.SignUp(user);
            }
            return null;
        }
        public int StrongPassword(User user)
        {
            var result = Zxcvbn.Core.EvaluatePassword(user.Password);
            return result.Score;
        }

    }
}
