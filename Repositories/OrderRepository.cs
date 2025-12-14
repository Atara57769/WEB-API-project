using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApiDBContext _apiDbContext;
        public OrderRepository(ApiDBContext apiDbContext)
        {
            _apiDbContext = apiDbContext;
        }
        public async Task<Order> GetOrderById(int id)
        {
            return await _apiDbContext.Orders.Include(order=>order.OrderItems).FirstOrDefaultAsync(order => order.Id==id);
        }

        public async Task<Order> AddOrder(Order newOrder)
        {
            await _apiDbContext.Orders.AddAsync(newOrder);
            await _apiDbContext.SaveChangesAsync();
            return newOrder;
        }
    }
}
