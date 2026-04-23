using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

    namespace e_commerce.app.Services.IServices
    {
        public interface IPhotoService
        {
        Task<string> UploadPhotoAsync(IFormFile file);

        Task<string> DeletePhotoAsync(string publicId);
    }
    }

