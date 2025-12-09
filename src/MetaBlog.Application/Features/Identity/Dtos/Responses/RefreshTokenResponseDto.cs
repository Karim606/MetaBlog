using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Identity.Dtos.Responses
{
    public class RefreshTokenResponseDto
    {
        public string AccessToken {  get; set; }
        public  string RefreshToken {  get; set; }
        public DateTime RefreshTokenExpiry { get; set; }

        public RefreshTokenResponseDto(string accessToken, string refreshToken, DateTime refreshTokenExpiry)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            RefreshTokenExpiry = refreshTokenExpiry;

        }

    }
}
