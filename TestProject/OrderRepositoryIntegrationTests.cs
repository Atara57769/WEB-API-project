using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class OrderRepositoryIntegrationTests :  IDisposable
    {
        private readonly DatabaseFixture _fixture;
        private readonly ApiDBContext _dbContext;
        private readonly OrderRepository _orderRepository;

        public OrderRepositoryIntegrationTests()
        {
            _fixture = new DatabaseFixture();
            _dbContext = _fixture.Context;
            _orderRepository = new OrderRepository(_dbContext);
        }

        public void Dispose()
        {
            _fixture.Dispose();
        }

        [Fact]
        public async Task GetOrderById_ReturnsOrderWithItems_WhenIdExists()
        {
            // Arrange
            var user = new User { FirstName = "Test User", Email = "test@example.com", Password = "123" };
            var category = new Category { Name = "General" };
            await _dbContext.Users.AddAsync(user);
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();

            var product = new Product { ProductName = "Test Product", CategoryId = category.Id, Price = 10.0 };
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();

            var order = new Order
            {
                UserId = user.Id,
                OrderDate = DateOnly.FromDateTime(DateTime.Now),
                OrderSum = 10.0,
                OrderItems = new List<OrderItem> { new OrderItem { ProductId = product.Id, Quantity = 1 } }
            };
            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _orderRepository.GetOrderById(order.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(order.Id, result.Id);
            Assert.Single(result.OrderItems);
        }

        [Fact]
        public async Task AddOrder_PersistsOrderAndItemsToDatabase()
        {
            // Arrange
            var user = new User { FirstName = "Buyer", Email = "buy@test.com", Password = "123" };
            var category = new Category { Name = "General" };
            await _dbContext.Users.AddAsync(user);
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();

            var product = new Product { ProductName = "Gadget", CategoryId = category.Id, Price = 100.0 };
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();

            var newOrder = new Order
            {
                UserId = user.Id,
                OrderDate = DateOnly.FromDateTime(DateTime.Now),
                OrderSum = 100.0,
                OrderItems = new List<OrderItem> { new OrderItem { ProductId = product.Id, Quantity = 1 } }
            };

            // Act
            var result = await _orderRepository.AddOrder(newOrder);

            // Assert
            Assert.NotEqual(0, result.Id);
            var dbOrder = await _dbContext.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == result.Id);
            Assert.NotNull(dbOrder);
            Assert.Single(dbOrder.OrderItems);
        }

        [Fact]
        public async Task GetOrderById_ReturnsNull_WhenIdDoesNotExist()
        {
            var result = await _orderRepository.GetOrderById(999);
            Assert.Null(result);
        }
    }
}
