using e_commerce.app.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace e_commerce.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IPhotoService _photoService;

        public PhotosController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        // POST: api/photos/upload
        [HttpPost("upload")]
        [Authorize (Roles ="Admin,Seller")]
        public async Task<IActionResult> UploadPhoto(IFormFile file)
        {
            try
            {
                var imageUrl = await _photoService.UploadPhotoAsync(file);

                // Return the URL as a JSON object
                return Ok(new { url = imageUrl });
            }
            catch (ArgumentException ex)
            {
                // Return 400 Bad Request for validation errors (e.g., wrong extension)
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Return 500 Internal Server Error for Cloudinary or server issues
                return StatusCode(500, new { message = "An error occurred while uploading the photo.", details = ex.Message });
            }
        }

        // DELETE: api/photos/delete/{publicId}
        [HttpDelete("delete/{publicId}")]
        [Authorize(Roles = "Admin,Seller")]
        public async Task<IActionResult> DeletePhoto(string publicId)
        {
            try
            {
                var resultMessage = await _photoService.DeletePhotoAsync(publicId);

                return Ok(new { message = resultMessage });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the photo.", details = ex.Message });
            }
        }
    }
}