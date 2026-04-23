
using e_commerce.app.Dto;
using e_commerce.app.servieses.iserviese;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;


namespace e_commerce.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var product = await _productService.GetByIdAsync(id);
                return Ok(product);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("seller/{sellerId}")]
        public async Task<IActionResult> GetBySeller(int sellerId)
        {
            var products = await _productService.GetBySellerAsync(sellerId);
            return Ok(products);
        }

        [HttpPost]
        [Authorize (Roles ="Seller")]
        public async Task<IActionResult> AddProduct([FromBody] CreateProductBySellerDto dto)
        {
            var sellerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await _productService.AddProductAsync(dto, sellerId);
            return Ok("Product created and waiting for approval");
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Seller,Admin")]
        public async Task<IActionResult> UpdateProduct(
            int id,
            [FromBody] UpdateProductBySellerDto dto)
        {
            try
            {
                await _productService.UpdateProductAsync(id, dto);
                return Ok("Product updated and waiting for approval again");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("approve/{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> ApproveProduct(
            int id,
            [FromBody] ApproveProductByAdminDto dto)
        {
            try
            {
                await _productService.ApproveProductAsync(id, dto);
                return Ok("Product approval updated");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles ="Admin,Seller")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                await _productService.DeleteProductAsync(id);
                return Ok("Product deleted");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // --- Endpoints الخاصة ببرانش محمود دياب ---

        [HttpPut("{id}/stock")]
        public async Task<IActionResult> UpdateStock(int id, int quantity)
        {
            try
            {
                await _productService.UpdateStockAsync(id, quantity);
                return Ok("Stock updated successfully");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}/stock")]
        public async Task<IActionResult> CheckStock(int id)
        {
            try
            {
                var product = await _productService.GetByIdAsync(id);
                return Ok(new
                {
                    id = product.Id,
                    name = product.Name,
                    quantity = product.Quantity
                });
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("low-stock")]
        public async Task<IActionResult> LowStock(int threshold = 5)
        {
            var result = await _productService.GetLowStockAsync(threshold);
            return Ok(result);
        }
    }
}