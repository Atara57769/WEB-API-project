using Entities;
using System.Text.Json;

namespace Repositories
{
    public class UserRepositories : IUserRepositories
    {
        private readonly string UsersFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Repositories", "users.txt");
        private readonly List<User> _users = new List<User>();

        public IEnumerable<User> GetUsers()
        {
            return _users;
        }

        public User GetUserById(int id)
        {
            using (StreamReader reader = System.IO.File.OpenText(UsersFilePath))
            {
                string? currentUserInFile;
                while ((currentUserInFile = reader.ReadLine()) != null)
                {
                    User user = JsonSerializer.Deserialize<User>(currentUserInFile);
                    if (user.Id == id)
                        return user;
                }
            }
            return null;
        }

        public User AddUser(User newUser)
        {
            int numberOfUsers = System.IO.File.ReadLines(UsersFilePath).Count();
            newUser.Id = numberOfUsers + 1;
            string userJson = JsonSerializer.Serialize(newUser);
            System.IO.File.AppendAllText(UsersFilePath, userJson + Environment.NewLine);
            return newUser;
        }

        public void UpdateUser(int id, User updateUser)
        {
            string textToReplace = string.Empty;
            using (StreamReader reader = System.IO.File.OpenText(UsersFilePath))
            {
                string currentUserInFile;
                while ((currentUserInFile = reader.ReadLine()) != null)
                {
                    User user = JsonSerializer.Deserialize<User>(currentUserInFile);
                    if (user.Id == id)
                        textToReplace = currentUserInFile;
                }
            }
            if (textToReplace != string.Empty)
            {
                string text = System.IO.File.ReadAllText(UsersFilePath);
                text = text.Replace(textToReplace, JsonSerializer.Serialize(updateUser));
                System.IO.File.WriteAllText(UsersFilePath, text);
            }
        }

        public User Login(LoginUser loginUser)
        {
            using (StreamReader reader = System.IO.File.OpenText(UsersFilePath))
            {
                string? currentUserInFile;
                while ((currentUserInFile = reader.ReadLine()) != null)
                {
                    User user = JsonSerializer.Deserialize<User>(currentUserInFile);
                    if (user.Email == loginUser.Email && user.Password == loginUser.Password)
                        return user;
                }
            }
            return null;
        }
    }
}
