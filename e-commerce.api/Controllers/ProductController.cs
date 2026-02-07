using Microsoft.AspNetCore.Mvc;
using e_commerce.app.servieses.iserviese;
using e_commerce.app.Dto;

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
        public async Task<IActionResult> AddProduct(
            [FromBody] CreateProductBySellerDto dto,
            [FromQuery] int sellerId)
        {
            await _productService.AddProductAsync(dto, sellerId);
            return Ok("Product created and waiting for approval");
        }
        [HttpPut("{id}")]
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


    }
}
