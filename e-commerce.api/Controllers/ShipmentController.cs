using e_commerce.app.Dto.ShipmentDTO;
using e_commerce.app.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipmentController : ControllerBase
    {
        private readonly IShipmentService _service;

        public ShipmentController(IShipmentService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ShipmentCreateDto dto)
        {
            await _service.CreateAsync(dto);
            return Ok("Shipment Created");
        }
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, ShipmentUpdateDto dto)
        {
            await _service.UpdateStatusAsync(id, dto);
            return Ok("Status Updated");
        }
    }
}
