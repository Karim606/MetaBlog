using MetaBlog.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<Result<object>> ChangePasswordAsync(string Email, string currentPassword, string newPassword);
        Task<Result<(Guid, List<string>)>> LoginAsync(string Email, string Password);
        Task<Result<Guid>> RegisterUserAsync(string Email, string password);
        Task<Result<List<String>>>GetUserRolesAsync(Guid Id);
        Task<Result<string>>GetUserEmailAsync(Guid Id);  
    }
}
