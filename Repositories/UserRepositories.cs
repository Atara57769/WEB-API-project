using Entities;
using System.Text.Json;

namespace Repositories
{
    public class UserRepositories : IUserRepositories
    {
        private List<User> _users = new List<User>();
        private string _filePath = "..\\Repositories\\users.txt";
        public IEnumerable<User> GetUsers()
        {
            return _users;
        }

        public User GetUserByID(int id)
        {
            using (StreamReader reader = System.IO.File.OpenText(_filePath))
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
            int numberOfUsers = System.IO.File.ReadLines(_filePath).Count();
            newUser.Id = numberOfUsers + 1;
            string userJson = JsonSerializer.Serialize(newUser);
            System.IO.File.AppendAllText(_filePath, userJson + Environment.NewLine);
            return newUser;
        }

        public User LoginUser(User loginUser)
        {
            using (StreamReader reader = System.IO.File.OpenText(_filePath))
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

        public void UpdateUser(int id, User updateUser)
        {
            string textToReplace = string.Empty;
            using (StreamReader reader = System.IO.File.OpenText(_filePath))
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
                string text = System.IO.File.ReadAllText(_filePath);
                text = text.Replace(textToReplace, JsonSerializer.Serialize(updateUser));
                System.IO.File.WriteAllText(_filePath, text);
            }
        }

        public User Login(LoginUser loginUser)
        {
            using (StreamReader reader = System.IO.File.OpenText(_filePath))
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
