using e_commerce.app.Dto.ProductReviewDTO;
using e_commerce.app.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductReviewController : ControllerBase
    {
        private readonly IReviewProductService _reviewProductService;
        public ProductReviewController(IReviewProductService reviewProductService)
        {
            _reviewProductService = reviewProductService;
        }
        [HttpPost]
        public async Task<IActionResult> AddAsync(AddProductReviewDTO productReviewDTO)
        {
            if (ModelState.IsValid)
            {
                await _reviewProductService.AddAsync(productReviewDTO);
                return Ok();
            }
            return BadRequest();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (ModelState.IsValid)
            {
                await _reviewProductService.DeleteAsync(id);
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            if (ModelState.IsValid)
            {
                var result = await _reviewProductService.GetByIdAsync(id);
                return Ok(result);
            }
            return BadRequest();
        }
        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetByProductIdAsync(int productId, bool onlyApproved = true)
        {
            if (ModelState.IsValid)
            {
                var result = await _reviewProductService.GetByProductIdAsync(productId, onlyApproved);
                return Ok(result);
            }
            return BadRequest();
            
        }
        [HttpPut("{reviewId}")]
        public async Task<IActionResult> UpdateAsync(int reviewId, UpdateProductReviewDTO productReview)
        {
            if (ModelState.IsValid)
            {
                var result = _reviewProductService.UpdateAsync(reviewId, productReview);
                return Ok(result);
            }
            return BadRequest();
        }

    }
}
