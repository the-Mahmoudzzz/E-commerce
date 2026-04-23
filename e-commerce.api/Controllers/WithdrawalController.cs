using e_commerce.app.Dto.WirhDrawlsDTO;
using e_commerce.app.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        [Authorize (Roles ="Seller")]
        public async Task<IActionResult> Request(CreateWithdrawalDto dto)
        {
            int sellerid = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var result = await _service.RequestWithdrawalAsync(sellerid,dto);
            return Ok(result);
        }

        [HttpPost("{id}/approve")]
        [Authorize (Roles ="Admin")]
        public async Task<IActionResult> Approve(int id)
        {
            await _service.ApproveWithdrawalAsync(id);
            return Ok();
        }
    }
}
