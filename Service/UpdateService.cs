using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;
using Entity;

namespace Service
{
   public class UpdateService
    {
        public UpdateRepository r = new UpdateRepository();
        public bool? Update(User user)
        {
            var result = Zxcvbn.Core.EvaluatePassword(user.Password);
            if (result.Score < 3)
                return null;
             return r.Update(user);
        }
    }
}
