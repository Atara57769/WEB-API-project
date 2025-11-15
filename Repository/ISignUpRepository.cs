using Entity;

namespace Repository
{
    public interface ISignUpRepository
    {
        User? SignUp(User user);
    }
}