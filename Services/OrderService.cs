using AutoMapper;
using DTOs;
using Entities;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        public OrderService(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;

        }
        public async Task<OrderDTO> GetOrderById(int id)
        {
            return _mapper.Map<Order, OrderDTO>(await _orderRepository.GetOrderById(id));
        }

        public async Task<OrderDTO> AddOrder(OrderDTO order)
        {
            return _mapper.Map<Order, OrderDTO>(await _orderRepository.AddOrder(_mapper.Map < OrderDTO, Order > (order)));
        }
    }
}
