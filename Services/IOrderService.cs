using Entities;

namespace Services
{
    public interface IOrderService
    {
        Task<Order> AddOrder(Order order);
        Task<Order> GetOrderById(int id);
    }
}