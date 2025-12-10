using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using MetaBlog.Application.Common.Interfaces;
using MetaBlog.Domain.Common.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Infrastructure.Services
{
    public class CloudinaryImageService:IImageService
    {
        private readonly Cloudinary _cloud;
        private readonly ILogger<CloudinaryImageService> _logger;
        private string _startMarker = "MetaBlog/";
        public CloudinaryImageService(ILogger<CloudinaryImageService>logger) {
            _cloud = new Cloudinary(Environment.GetEnvironmentVariable("CLOUDINARY_URL"));
            _cloud.Api.Secure = true;

            _logger = logger;
        }

        public async Task<Result<string>> UploadAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            await using var stream = file.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "MetaBlog"
            };

            try
            {
                var uploadResult = await _cloud.UploadAsync(uploadParams);
                return uploadResult.SecureUrl.ToString();
            }
            catch (Exception e)
            {
                
                return Domain.Common.Results.Error.Failure(description: "Image upload failed.");
                _logger.LogError(e, "Image upload failed for file {FileName}", file.FileName);
            }
        }

        public async Task<Result<bool>> DeleteAsync(string secureUrl)
        {
            int indexOf = secureUrl.IndexOf(_startMarker);
            string publicId = secureUrl.Substring(indexOf, secureUrl.Length - indexOf);

            var deleteParams = new DeletionParams(publicId);
            try
            {
                var result = await _cloud.DestroyAsync(deleteParams);
                return result.Result == "ok";
            }
            catch (Exception e)
            {

                return Domain.Common.Results.Error.Failure(description: "Image Deletion failed.");
                _logger.LogError(e, "Image Deletion failed for file with publicId : {publicId}",publicId);
            }
           
        }


    }
}
