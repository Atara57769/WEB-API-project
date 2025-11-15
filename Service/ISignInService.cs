using Entity;

namespace Service
{
    public interface ISignInService
    {
        User? SignIn(SignIn user);
    }
}