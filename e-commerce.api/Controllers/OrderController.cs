using e_commerce.app.Dto.OrderDto;
using e_commerce.app.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

      
        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderCreateDto orderDto)
        {
            await _orderService.CreateOrder(orderDto);
            return Ok(new { message = "Order Created Successfully" });
        }

        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<IReadOnlyList<OrderDTO>>> GetOrdersByCustomer(int customerId)
        {
            var orders = await _orderService.GetOrderByCustomerId(customerId);
            return Ok(orders);
        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult<OrderDTO>> GetOrderById(int orderId)
        {
            var order = await _orderService.GetOrderById(orderId);

            if (order == null)
                return NotFound();

            return Ok(order);
        }
        [Authorize(Roles ="Seller")]
        [HttpGet("seller")]
        public async Task<ActionResult<OrderDTO>> GetSellerOrder()
        {
            var order = await _orderService.GetIncomingOrder();

            if (order == null)
                return NotFound();

            return Ok(order);
        }

    }
}

