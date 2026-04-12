using e_commerce.app.Dto;
using e_commerce.app.Dto.UserAddressDto;
using e_commerce.app.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace e_commerce.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserAddressController : ControllerBase
    {
        private readonly IUserAddressService _addressService;

        public UserAddressController(IUserAddressService addressService)
        {
            _addressService = addressService;
        }

        private int UserId =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        [HttpGet]
        public async Task<IActionResult> GetAddresses()
        {
            var addresses = await _addressService.GetUserAddressesAsync(UserId);
            return Ok(addresses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAddress(int id)
        {
            var address = await _addressService.GetAddressByIdAsync(UserId, id);

            if (address == null)
                return NotFound(new { message = "Address not found" });

            return Ok(address);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAddress([FromBody] CreateUserAddressDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _addressService.CreateAddressAsync(UserId, dto);

            return CreatedAtAction(
                nameof(GetAddress),
                new { id = result.Id },
                result
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAddress(int id, [FromBody] UpdateUserAddressDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _addressService.UpdateAddressAsync(UserId, id, dto);

            if (result == null)
                return NotFound(new { message = "Address not found" });

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            var deleted = await _addressService.DeleteAddressAsync(UserId, id);

            if (!deleted)
                return NotFound(new { message = "Address not found" });

            return NoContent();
        }

        [HttpPatch("{id}/set-default")]
        public async Task<IActionResult> SetDefault(int id)
        {
            var updated = await _addressService.SetDefaultAddressAsync(UserId, id);

            if (!updated)
                return NotFound(new { message = "Address not found" });

            return Ok(new { message = "Default address updated successfully" });
        }
    }
}