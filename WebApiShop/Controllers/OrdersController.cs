using Entities;
using Microsoft.AspNetCore.Mvc;
using Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        // GET api/<OrdersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> Get(int id)
        {
            Order order = await _orderService.GetOrderById(id);
            if (order == null)
                return NotFound();
            return Ok(order);
        }

        // POST api/<OrdersController>
        [HttpPost]
        public async Task<ActionResult<Order>> Post([FromBody] Order newOrder)
        {
            newOrder = await _orderService.AddOrder(newOrder);
            if (newOrder == null)
                return BadRequest();
            return CreatedAtAction(nameof(Get), new { id = newOrder.Id }, newOrder);
        }

    }
}
