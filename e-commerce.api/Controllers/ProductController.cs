using e_commerce.app.Dto.ProductDto;
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
                var product = await _productService
                    .GetByIdAsync(id);
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
            var products = await _productService
                .GetAllAsync();
            return Ok(products);
        }

        [HttpGet("seller/{sellerId}")]
        public async Task<IActionResult> GetBySeller(int sellerId)
        {
            var products = await _productService
                .GetBySellerAsync(sellerId);

            return Ok(products);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddProduct(
            [FromBody] CreateProductBySellerDto dto,
            [FromQuery] int sellerId)
        {
            await _productService.AddProductAsync(dto, sellerId);

            return Ok("Product created and waiting for approval");
        }
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductBySellerDto dto)
        {
            try
            {  
                var sellerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                await _productService.UpdateProductAsync(id, dto, sellerId);
                return Ok("Product updated and waiting for approval again");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
        [Authorize]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var sellerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                await _productService.DeleteProductAsync(id, sellerId);

                return Ok("Product deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] ProductSearchDto searchParams)
        {
            try
            {
                var result = await _productService.SearchAsync(searchParams);

                return Ok(new
                {
                    Data = result.Products,
                    TotalCount = result.TotalCount,
                    Page = searchParams.Page,
                    PageSize = searchParams.PageSize
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
