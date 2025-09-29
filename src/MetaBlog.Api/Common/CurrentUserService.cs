using MetaBlog.Application.Common.Interfaces;
using MetaBlog.Domain.Users;
using System.Security.Claims;

namespace MetaBlog.Api.Common
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly ClaimsPrincipal? _user;
        public CurrentUserService(IHttpContextAccessor? http)
        {
            _user = http?.HttpContext?.User;
            if (_user == null)
            {
                throw new InvalidOperationException("User context is not present");
            }


        }
        public string? GetEmail()
        {
            return _user?.FindFirst(ClaimTypes.Email)?.Value;
        }

        public string GetId()
        {
            var id = _user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return id;
        }

        public List<string> GetRoles()
        {
           var roles = _user?.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList() ?? new List<string>();
            return roles;
        }

        public string GetUserName()
        {
            return _user?.FindFirst(ClaimTypes.Name)?.Value ?? "Unknown";

        }
    }
}
