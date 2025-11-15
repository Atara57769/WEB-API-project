using Entity;

namespace Repository
{
    public interface ISignInRepository
    {
        User? SignIn(SignIn user1);
    }
}