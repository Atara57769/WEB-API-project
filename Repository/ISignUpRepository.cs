using Entity;

namespace Repository
{
    public interface ISignUpRepository
    {
        Task<User?> SignUp(User user);
    }
}