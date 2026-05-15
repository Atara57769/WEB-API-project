using AutoMapper;
using DTOs;
using Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.EntityFrameworkCore;
using Repositories;
using Services;

namespace TestProject
{
    public class OrderServiceUnitTests
    {
        [Fact]
        public async Task AddOrder_ReturnsOrderDTO_WhenSumIsCorrect()
        {
            // Arrange
            var orderItems = new List<OrderItemDTO>
    {
        new OrderItemDTO(0, 1, 2)
    };

            var orderDto = new OrderDTO(
                0,
                1,
                DateOnly.FromDateTime(DateTime.Now),
                100.0,
                orderItems
            );

            var product = new Product { Id = 1, Price = 50.0 };
            var orderEntity = new Order { Id = 0, OrderSum = 100.0 };

            var mockOrderRepo = new Mock<IOrderRepository>();
            var mockProductRepo = new Mock<IProductRepository>();
            var mockMapper = new Mock<IMapper>();

            mockProductRepo.Setup(x => x.GetProductById(1)).ReturnsAsync(product);

            mockMapper.Setup(m => m.Map<OrderDTO, Order>(orderDto)).Returns(orderEntity);
            mockMapper.Setup(m => m.Map<Order, OrderDTO>(orderEntity)).Returns(orderDto);

            mockOrderRepo.Setup(x => x.AddOrder(It.IsAny<Order>())).ReturnsAsync(orderEntity);
            var mockLogger = new Mock<ILogger<OrderService>>();
            var mockKafkaProducer = new Mock<IKafkaProducerService>();
            var mockConfig = new Mock<IConfiguration>();

            var service = new OrderService(mockOrderRepo.Object,mockProductRepo.Object,mockMapper.Object,mockLogger.Object, mockKafkaProducer.Object, mockConfig.Object);
            // Act
            var result = await service.AddOrder(orderDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(orderDto.OrderSum, result.OrderSum);
            mockOrderRepo.Verify(x => x.AddOrder(It.IsAny<Order>()), Times.Once);

        }
        [Fact]
        public async Task AddOrder_ReturnsNull_WhenSumIsIncorrect()
        {
            // Arrange
            var orderItems = new List<OrderItemDTO>
            {
                new OrderItemDTO(0, 1, 2)
            };

            var orderDto = new OrderDTO(
                0,
                1,
                DateOnly.FromDateTime(DateTime.Now),
                999.0,            
                orderItems
            );

            var product = new Product { Id = 1, Price = 50.0 };

            var mockOrderRepo = new Mock<IOrderRepository>();
            var mockProductRepo = new Mock<IProductRepository>();
            var mockMapper = new Mock<IMapper>();

            mockProductRepo.Setup(x => x.GetProductById(1)).ReturnsAsync(product);
            var mockLogger = new Mock<ILogger<OrderService>>();
            var mockKafkaProducer = new Mock<IKafkaProducerService>();
            var mockConfig = new Mock<IConfiguration>();

            var service = new OrderService(mockOrderRepo.Object, mockProductRepo.Object, mockMapper.Object, mockLogger.Object, mockKafkaProducer.Object, mockConfig.Object);

            // Act
            var result = await service.AddOrder(orderDto);

            // Assert
            Assert.Null(result);

            
            mockOrderRepo.Verify(x => x.AddOrder(It.IsAny<Order>()), Times.Never);
        }
    }
}
