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
        [HttpGet("Sub")]
        public async Task<IActionResult> GetAllSub()
        {
            var Categotis = await categoryService.GetAllSubCategoryAsync();
            return Ok(Categotis);
        }
        [HttpGet("type/{type}")]
        public async Task<IActionResult> Get(int id) { 
            var category= await categoryService.GetbyIdAsync(id);
            return Ok(category);
        }
        [HttpGet("typesub/{type}")]
        public async Task<IActionResult> GetSub(int id)
        {
            var category = await categoryService.GetbyIdSubCategoryAsync(id);
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
        [HttpPost("sub")]
        public async Task<IActionResult> CreateSub(CreateSubCategoryDto category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await categoryService.AddSubCategoryAsync(category);
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
        [HttpPut("sub")]
        public IActionResult EditSub(CreateSubCategoryDto createCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var updated = categoryService.UpdateSubCategoryAsync(createCategoryDto);
            return Ok(updated);

        }
        [HttpDelete]
        public IActionResult Delete(int id) {
            categoryService.DeleteAsync(id);
            return Ok();
        }

    }
}
