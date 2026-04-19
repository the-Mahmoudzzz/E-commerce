using e_commerce.app.Dto.WirhDrawlsDTO;
using e_commerce.app.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WithdrawalController : ControllerBase
    {
        private readonly IWithdrawalService _service;

        public WithdrawalController(IWithdrawalService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Request(CreateWithdrawalDto dto)
        {
            var result = await _service.RequestWithdrawalAsync(dto);
            return Ok(result);
        }

        [HttpPost("{id}/approve")]
        public async Task<IActionResult> Approve(int id)
        {
            await _service.ApproveWithdrawalAsync(id);
            return Ok();
        }
    }
}
