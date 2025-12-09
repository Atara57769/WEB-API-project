using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Entity;


namespace Repository
{
    public class SignInRepository : ISignInRepository
    {
        public async Task<User?> SignIn(SignIn user1)
        {
            using (StreamReader reader = System.IO.File.OpenText("C:\\Users\\aliza.twito\\Documents\\ruti\\WEB\\MyWebApiProject\\DataFile.txt"))
            {
                string? currentUserInFile;
                while ((currentUserInFile = await reader.ReadLineAsync()) != null)
                {
                    if (string.IsNullOrWhiteSpace(currentUserInFile))
                        continue;
                    if (!currentUserInFile.Trim().StartsWith("{"))
                        continue;
                    User? user = JsonSerializer.Deserialize<User>(currentUserInFile);
                    if (user?.UserName == user1.UserName1 && user?.Password == user1.Password1)
                        return user;
                }
            }
            return null;
        }
    }
}
