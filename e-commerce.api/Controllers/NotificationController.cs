using e_commerce.app.Dto.NotificationDto;
using e_commerce.app.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace e_commerce.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
       // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllNotification()
        {
            return Ok(await _notificationService.GetALLAsync());
        }

        [HttpGet("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetNotification(int id)
        {
            return Ok(await _notificationService.GetByidAsync(id));
        }

        [HttpGet("my-notifications")]
        public async Task<IActionResult> GetAllUserNotification()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out int userId)) return Unauthorized();

            return Ok(await _notificationService.GetALLUserNotifiAsync(userId));
        }

        [HttpPut("{id}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out int userId)) return Unauthorized();

            await _notificationService.MarkAsReadAsync(userId, id);
            return Ok();
        }

        [HttpPut("read-all")]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out int userId)) return Unauthorized();

            await _notificationService.MarkAllAsReadAsync(userId);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            await _notificationService.Delete(id);
            return Ok();
        }
    }
}