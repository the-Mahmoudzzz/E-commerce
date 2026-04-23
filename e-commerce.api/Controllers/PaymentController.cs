using e_commerce.app.Dto.PayMentDTO;
using e_commerce.app.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _service;

        public PaymentController(IPaymentService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePaymentDto dto)
        {
            var result = await _service.CreatePaymentAsync(dto);
            return Ok(result);
        }

        [HttpPost("callback")]
        public async Task<IActionResult> Callback([FromBody] dynamic data)
        {
            await _service.HandleCallbackAsync(data);
            return Ok();
        }
    }
    }
