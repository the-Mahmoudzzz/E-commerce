using e_commerce.app.Dto;
using e_commerce.app.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountService _discountService;

        public DiscountController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var discounts = await _discountService.GetAllAsync();
            return Ok(discounts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var discount = await _discountService.GetByIdAsync(id);
            return Ok(discount);
        }

        [HttpPost("apply")]
        [Authorize]
        public async Task<IActionResult> ApplyDiscount([FromBody] ApplyDiscountDto dto)
        {
            var result = await _discountService.ApplyDiscountAsync(dto.Code, dto.OrderTotal);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles ="Seller,Admin")]
        public async Task<IActionResult> Create([FromBody] CreateDiscountDto dto)
        {
            await _discountService.AddAsync(dto);
            return Ok("Created");
        }

        [HttpPut]
        [Authorize (Roles ="Seller,Admin")]
        public async Task<IActionResult> Update([FromBody] UpdateDiscountDto dto)
        {
            await _discountService.UpdateAsync(dto);
            return Ok("Updated");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles ="Admin,Seller")]
        public async Task<IActionResult> Delete(int id)
        {
            await _discountService.DeleteAsync(id);
            return Ok("Deleted");
        }
    }
}