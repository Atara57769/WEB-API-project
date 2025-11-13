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
   public class SignUpRepository
    {
        public User? SignUp(User user)
        {
            
                int numberOfUsers = System.IO.File.ReadLines("C:\\Users\\aliza.twito\\Documents\\ruti\\WEB\\MyWebApiProject\\DataFile.txt").Count();
                user.UserId = numberOfUsers + 1;
                string userJson = JsonSerializer.Serialize(user);
                System.IO.File.AppendAllText("C:\\Users\\aliza.twito\\Documents\\ruti\\WEB\\MyWebApiProject\\DataFile.txt", userJson + Environment.NewLine);
                return  user;
            
            
        }
    }
}
