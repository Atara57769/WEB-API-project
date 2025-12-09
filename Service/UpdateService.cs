using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;
using Entity;

namespace Service
{
    public class UpdateService : IUpdateService
    {
        private readonly IUpdateRepository _repo;

        public UpdateService(IUpdateRepository repo)
        {
            _repo = repo;
        }
        public async Task<bool?> Update(User user)
        {
            return await _repo.Update(user);
        }
    }
}
