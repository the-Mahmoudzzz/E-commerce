using e_commerce.app.Dto;
using e_commerce.app.Dto.FeedBackDTO;
using e_commerce.app.Services.IServices;
using e_commerce.core.entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService feedbackService;
        public FeedbackController(IFeedbackService feedbackService)
        {
            this.feedbackService = feedbackService;

        }
        [HttpPost]
        [Authorize(Roles ="User,Customer")]
        public async Task<IActionResult> Create(CreateFeedbackDto createFeedbackDto) {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {

               await feedbackService.CreateAsync(createFeedbackDto);
                return Ok();
            }
        }
        [HttpGet]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParamsDto pagination) { 
        
        
            var result=  await feedbackService.GetAllAsync(pagination);
            return Ok(result);
        }

        [HttpGet("type/{type}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetByType(FeedbackType type)
        {
            var result = await feedbackService.GetByTypeAsync(type);
            return Ok(result);
        }
    }
}
