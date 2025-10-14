using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetaBlog.Domain.RefreshTokens;
using Microsoft.AspNetCore.Identity;

namespace MetaBlog.Infrastructure.Identity
{
    public class IdentityAppUser:IdentityUser<Guid>
    { 

    }
}

