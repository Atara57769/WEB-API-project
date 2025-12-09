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

        public async Task<User?> SignIn(SignIn user)
        {
            return await _repo.SignIn(user);
        }

    }
}
