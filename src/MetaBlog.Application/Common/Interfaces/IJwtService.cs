using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Common.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(string Name, string Email, Guid Id, List<string> Roles);
        (string, DateTime) GenerateRefreshToken();
        public string HashToken(string token);
    }
}
