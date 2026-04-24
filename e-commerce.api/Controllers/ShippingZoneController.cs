using e_commerce.app.Dto.ZondeDTO;
using e_commerce.app.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace e_commerce.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ShippingZoneController : ControllerBase
    {
        private readonly IShippingService _shippingService;
        public ShippingZoneController(IShippingService shippingService)
        {
            _shippingService = shippingService;
        }
        [HttpGet]
        public async Task<IActionResult> Get() {
            var zones =await _shippingService.GetAllZonesAsync();
            return Ok(zones);
            
        }
        [HttpGet("type/{type}")]
        public async Task<IActionResult> GetById(int id) {
            var zone =await _shippingService.GetZoneAsync(id);
            return Ok(zone);
            
           
        }
        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> AddZone(ShippingZoneDto zoneDto)
        {
            await _shippingService.AddZoneAsync(zoneDto);
            return Created();
        }
        [HttpPut]
        public async Task<IActionResult> EditZone( int id,UpdateZoneDto zoneDto) {
            await _shippingService.UpdateZoneAsync(id,zoneDto);
            return Ok();
        }
        [HttpDelete]
        public async Task  DeleteZone(int id) { 
            await _shippingService.DeleteZoneAsync(id);
            
        }
    }
}
