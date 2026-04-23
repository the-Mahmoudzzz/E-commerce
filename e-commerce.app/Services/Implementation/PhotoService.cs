using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using e_commerce.app.External;
using e_commerce.app.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace e_commerce.app.Services.Implementation
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;

        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var account = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadPhotoAsync(IFormFile file)
        {
            // 1. Check if the file is valid
            if (file == null || file.Length == 0)
                throw new ArgumentException("Invalid or empty file.");

            // 2. Validate file extension
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
                throw new ArgumentException("Unsupported file format. Only JPG, PNG, and WEBP are allowed.");

            var uploadResult = new ImageUploadResult();

            //  Upload the file
            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Transformation = new Transformation().Width(800).Height(800).Crop("fill").Gravity("auto")
            };

            uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
                throw new Exception(uploadResult.Error.Message);

            //  Return secure URL
            return uploadResult.SecureUrl.ToString();
        }


        public async Task<string> DeletePhotoAsync(string publicId)
        {
            if (string.IsNullOrEmpty(publicId))

                throw new ArgumentException("A valid Public Id is required for deletion.");

            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);

            if (result.Result == "ok")
            {
                return "Deleted successfully.";
            }

            throw new Exception($"Error deleting photo from Cloudinary: {result.Result}");
        }
    }
}