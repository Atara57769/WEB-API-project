using AutoMapper;
using DTOs;
using Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Repositories;

namespace Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderService> _logger;
        private readonly IKafkaProducerService _kafkaProducerService;
        private readonly IConfiguration _configuration;

        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository, IMapper mapper, ILogger<OrderService> logger, IKafkaProducerService kafkaProducerService, IConfiguration configuration)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _mapper = mapper;
            _logger = logger;
            _kafkaProducerService = kafkaProducerService;
            _configuration = configuration;
        }

        public async Task<OrderDTO> GetOrderById(int id)
        {
            return _mapper.Map<Order, OrderDTO>(await _orderRepository.GetOrderById(id));
        }

        public async Task<OrderDTO> AddOrder(OrderDTO order)
        {
            if (await CheckOrderSum(order))
            {
                var addedOrder = await _orderRepository.AddOrder(_mapper.Map<OrderDTO, Order>(order));
                var orderDto = _mapper.Map<Order, OrderDTO>(addedOrder);
                
                await _kafkaProducerService.PublishMessageAsync(_configuration["Kafka:Topic"] ?? "orders", orderDto);
                
                return orderDto;
            }
            _logger.LogWarning("user id:" + order.UserId + "tried to close order with unmatched sum");
            return null;
        }

        private async Task<bool> CheckOrderSum(OrderDTO order)
        {
            double? sum = 0;
            foreach (var item in order.OrderItems)
            {
                Product product =await _productRepository.GetProductById(item.ProductId);
                if (product != null) 
                    sum += product.Price * item.Quantity;
            }
            if(sum==order.OrderSum)
                return true;
            return false;
        }
    }
}
