using MetaBlog.Domain.Common.Results;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Common.Interfaces
{
    public interface IImageService
    {
        Task<Result<string>> UploadAsync(IFormFile file);
        Task<Result<bool>> DeleteAsync(string imageUrl);
    }
}
