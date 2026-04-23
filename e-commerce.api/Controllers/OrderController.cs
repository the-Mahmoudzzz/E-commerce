using e_commerce.app.Dto.OrderDto;
using e_commerce.app.Services.IServices;
using e_commerce.core.entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        [Authorize (Roles ="User,Customer")]
        public async Task<IActionResult> CreateOrder(OrderCreateDto orderDto)
        {
            await _orderService.CreateOrder(orderDto);
            return Ok(new { message = "Order Created Successfully" });
        }

        [HttpGet("customer")]
        [Authorize(Roles = "User,Customer")]
        public async Task<ActionResult<IReadOnlyList<OrderDTO>>> GetOrdersByCustomer()
        {
            int customerid = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var orders = await _orderService.GetOrderByCustomerId(customerid);

            return Ok(orders);
        }

        [HttpGet("{orderId}")]
        [Authorize (Roles ="Admin")]
        public async Task<ActionResult<OrderDTO>> GetOrderById(int orderId)
        {
            var order = await _orderService.GetOrderById(orderId);

            if (order == null)
                return NotFound("No Order Found");
            
            return Ok(order);
        }
        [Authorize(Roles = "Seller")]
        [HttpGet("seller")]
        public async Task<ActionResult<OrderDTO>> GetSellerOrder()
        {
            var order = await _orderService.GetIncomingOrder();

            if (order == null)
                return NotFound("No Order Yet");

            return Ok(order);
        }

        [HttpPut("CancedlOrder")]
        [Authorize(Roles ="User,Customer")]
        public async Task<IActionResult> CancelOrder (int orderid)
        {
            int customerid = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            try
            {
                await _orderService.CancelOrder(customerid,orderid);
            }
            catch (Exception ex) { 
                return BadRequest(ex.Message);
            }
           

            return Ok("Order is Canceld");
        }

    }
}

