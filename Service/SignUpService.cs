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
        public User? SignUp(User user)
        {
            if (user.FirstName != "" && user.LastName != "" && user.Password != "" && user.UserName != "")
            {
                var result = Zxcvbn.Core.EvaluatePassword(user.Password);
                if (result.Score < 3)
                    return null;
                return _repo.SignUp(user);
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
