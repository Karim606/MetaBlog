using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Domain.RefreshTokens
{
    public enum RevokeReasons
    {
        Rotated = 0,
        LoggedOut = 1

    }
}
