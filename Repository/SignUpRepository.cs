using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Entity;


namespace Repository
{
    public class SignUpRepository : ISignUpRepository
    {
        public async Task<User?> SignUp(User user)
        {
            int numberOfUsers = (await System.IO.File.ReadAllLinesAsync("C:\\Users\\aliza.twito\\Documents\\ruti\\WEB\\MyWebApiProject\\DataFile.txt")).Length;
            user.UserId = numberOfUsers + 1;
            string userJson = JsonSerializer.Serialize(user);
            await System.IO.File.AppendAllTextAsync("C:\\Users\\aliza.twito\\Documents\\ruti\\WEB\\MyWebApiProject\\DataFile.txt", userJson + Environment.NewLine);
            return user;
        }
    }
}
