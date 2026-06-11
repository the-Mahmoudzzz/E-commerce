using e_commerce.app.Dto.PayMentDTO;
using e_commerce.app.Services.Implementation;
using e_commerce.app.Services.IServices;
using e_commerce.core.entities;
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
        private readonly IPaymobService _paymobservice;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IPaymentService service, IPaymobService paymobservice, ILogger<PaymentController> logger)
        {
            _service = service;
            _paymobservice = paymobservice;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePaymentDto dto)
        {
            var result = await _service.CreatePaymentAsync(dto);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("webhook")]
        public async Task<IActionResult> PaymobWebhook([FromQuery] string hmac, [FromBody] PaymobCallbackDto callbackData)
        {
            
            if (string.IsNullOrEmpty(hmac))
                return Unauthorized("HMAC is missing.");

            
            bool isValid = await _paymobservice.ValidateHmac(callbackData,hmac);

            if (!isValid)
            {
                _logger.LogWarning("Invalid HMAC received for Order: {OrderId}", callbackData.Obj.Order.Id);
                return Unauthorized("Invalid HMAC Signature."); 
            }
            await _service.HandleCallbackAsync(callbackData);
            return Ok();
        }
    }
    }
