using Entity;

namespace Service
{
    public interface ISignInService
    {
        Task<User?> SignIn(SignIn user);
    }
}