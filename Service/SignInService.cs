using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;
using Entity;

namespace Service
{
    public  class SignInService
    {
        public SignInRepository r = new SignInRepository();
        public User? SignIn(SignIn user)
        {
            return r.SignIn(user);
        }

    }
}
