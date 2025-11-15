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
    public class UpdateRepository : IUpdateRepository
    {
        public bool Update(User user)
        {
            string textToReplace = string.Empty;
            using (StreamReader reader = System.IO.File.OpenText("C:\\Users\\aliza.twito\\Documents\\ruti\\WEB\\MyWebApiProject\\DataFile.txt"))
            {
                string? currentUserInFile;
                while ((currentUserInFile = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(currentUserInFile))
                        continue;
                    if (!currentUserInFile.Trim().StartsWith("{"))
                        continue;

                    User? user1 = JsonSerializer.Deserialize<User>(currentUserInFile);
                    if (user.UserId == user1?.UserId)
                        textToReplace = currentUserInFile;
                }
            }

            if (textToReplace != string.Empty)
            {
                string text = System.IO.File.ReadAllText("C:\\Users\\aliza.twito\\Documents\\ruti\\WEB\\MyWebApiProject\\DataFile.txt");

                text = text.Replace(textToReplace, JsonSerializer.Serialize(user));
                System.IO.File.WriteAllText("C:\\Users\\aliza.twito\\Documents\\ruti\\WEB\\MyWebApiProject\\DataFile.txt", text);
                return true;
            }
            return false;
        }
    }
}
