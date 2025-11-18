using Entity;

namespace Service
{
    public interface ISignUpService
    {
        User? SignUp(User user);
        int StrongPassword(User user);
    }
}