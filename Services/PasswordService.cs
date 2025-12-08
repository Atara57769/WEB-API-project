
using Zxcvbn;
namespace Services
{
    public class PasswordService : IPasswordService
    {
        public int GetPasswordScore(string password)
        {
            var result = Zxcvbn.Core.EvaluatePassword(password);
            return result.Score;
        }
    }
}
