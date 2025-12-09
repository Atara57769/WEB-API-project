using Entity;

namespace Repository
{
    public interface ISignInRepository
    {
        Task<User?> SignIn(SignIn user1);
    }
}