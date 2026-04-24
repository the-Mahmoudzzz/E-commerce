using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using e_commerce.app.External;
using e_commerce.app.Services.IServices;
using e_commerce.core.Exceptions;          // ← ضيف ده
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace e_commerce.app.Services.Implementation
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;

        // ✅ Constants — سهل تتغير في مكان واحد
        private const long MaxFileSizeBytes = 5 * 1024 * 1024;  // 5MB
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };

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
            // ✅ الملف مش موجود
            if (file == null || file.Length == 0)
                throw new ValidationException("File", "Please provide a valid image file.");

            // ✅ الحجم أكبر من المسموح
            if (file.Length > MaxFileSizeBytes)
                throw new ValidationException("File", $"File size cannot exceed {MaxFileSizeBytes / (1024 * 1024)}MB.");

            // ✅ Extension مش مسموح بيها
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (!AllowedExtensions.Contains(extension))
                throw new ValidationException("File",
                    $"Unsupported format '{extension}'. Allowed formats: {string.Join(", ", AllowedExtensions)}.");

            using var stream = file.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Transformation = new Transformation()
                    .Width(800).Height(800).Crop("fill").Gravity("auto")
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            // ✅ Cloudinary رجع error
            if (uploadResult.Error != null)
                throw new BusinessRuleException($"Image upload failed: {uploadResult.Error.Message}");

            return uploadResult.SecureUrl.ToString();
        }

        public async Task<string> DeletePhotoAsync(string publicId)
        {
            // ✅ publicId فاضي
            if (string.IsNullOrWhiteSpace(publicId))
                throw new ValidationException("PublicId", "A valid public ID is required for deletion.");

            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);

            // ✅ الصورة مش موجودة في Cloudinary
            if (result.Result == "not found")
                throw new NotFoundException("Photo", publicId);

            // ✅ Cloudinary رجع error
            if (result.Result != "ok")
                throw new BusinessRuleException($"Failed to delete photo from Cloudinary: {result.Result}");

            return "Photo deleted successfully.";
        }
    }
}