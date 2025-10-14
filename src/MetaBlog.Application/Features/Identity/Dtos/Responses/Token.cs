using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Identity.Dto.Responses
{
    public class Token
    {
        public string AccessToken { get; set; } = string.Empty;

        public string RefreshToken {  get; set; } = string.Empty;
        public DateTime RefreshTokenExpiry { get; set; }

        public Token(string accessToken,string refreshToken,DateTime refreshTokenExpiry)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            RefreshTokenExpiry = refreshTokenExpiry;

        }
    }
}
