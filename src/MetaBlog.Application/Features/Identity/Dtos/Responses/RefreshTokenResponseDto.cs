using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Identity.Dtos.Responses
{
    public class RefreshTokenResponseDto
    {
       public string accessToken {  get; set; }
        public  string refreshToken {  get; set; }
        public DateTime expiresAt { get; set; }

    }
}
