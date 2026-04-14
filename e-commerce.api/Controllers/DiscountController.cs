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
        public async Task<IActionResult> ApplyDiscount([FromBody] ApplyDiscountDto dto)
        {
            var discount = await _discountService
                .ApplyDiscountAsync(dto.Code, dto.OrderTotal);

            return Ok(discount);
        }

        
        [HttpPost]
        [Authorize] 
        public async Task<IActionResult> Create([FromBody] DiscountDto dto)
        {
            await _discountService.AddAsync(dto);
            return Ok("Discount created successfully");
        }

    
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] DiscountDto dto)
        {
            await _discountService.UpdateAsync(dto);
            return Ok("Discount updated successfully");
        }

      
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            await _discountService.DeleteAsync(id);
            return Ok("Discount deleted successfully");
        }
    }
}