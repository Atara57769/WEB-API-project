using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;
using Entity;

namespace Service
{
    public class SignInService : ISignInService
    {
        private readonly ISignInRepository _repo;

        public SignInService(ISignInRepository repo)
        {
            _repo = repo;
        }
        public SignInRepository r = new SignInRepository();
        public User? SignIn(SignIn user)
        {
            return  _repo.SignIn(user);
        }

    }
}
