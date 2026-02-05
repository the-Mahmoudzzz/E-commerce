using e_commerce.app.Dto.CtegoriesDto;
using e_commerce.app.Services.IServices;
using e_commerce.core.entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace e_commerce.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll() { 
            var Categotis=  await categoryService.GetAllAsync();
            return Ok(Categotis);
        }
        [HttpGet("type/{type}")]
        public async Task<IActionResult> Get(int id) { 
            var category= await categoryService.GetbyIdAsync(id);
            return Ok(category);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryDto category) { 
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            await categoryService.AddAsync(category);
            return Created();
        }
        [HttpPut]
        public IActionResult Edit(CreateCategoryDto createCategoryDto) {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var updated= categoryService.UpdateAsync(createCategoryDto);
            return Ok(updated);

        }
        [HttpDelete]
        public IActionResult Delete(int id) {
            categoryService.DeleteAsync(id);
            return Ok();
        }

    }
}
