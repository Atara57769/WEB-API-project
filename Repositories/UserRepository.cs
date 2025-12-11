using Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApiDBContext _apiDbContext;

        public UserRepository(ApiDBContext apiDbContext)
        {
            _apiDbContext = apiDbContext;
        }
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _apiDbContext.Users.Include(user=>user.Orders).ToListAsync();
        }

        public async Task<User> GetUserById(int id)
        {
            return await _apiDbContext.Users.FindAsync(id);
        }

        public async Task<User> AddUser(User newUser)
        {
            await _apiDbContext.Users.AddAsync(newUser);
            await _apiDbContext.SaveChangesAsync();
            return newUser;
        }

        public async Task UpdateUser(int id, User updateUser)
        {
            _apiDbContext.Users.Update(updateUser);
            await _apiDbContext.SaveChangesAsync();
        }

        public async Task<User> Login(User loginUser)
        {
            return await _apiDbContext.Users.Include(user=>user.Orders).FirstOrDefaultAsync(user => user.Email == loginUser.Email && user.Password == loginUser.Password);
        }

        public async Task<User> IsEmailExists(string email)
        {
            User userWithSameEmail = await _apiDbContext.Users.FirstOrDefaultAsync(user => user.Email == email);
            return userWithSameEmail;

        }
    }
}
