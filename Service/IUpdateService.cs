using Entity;

namespace Service
{
    public interface IUpdateService
    {
        Task<bool?> Update(User user);
    }
}