using Entity;

namespace Repository
{
    public interface IUpdateRepository
    {
        Task<bool> Update(User user);
    }
}