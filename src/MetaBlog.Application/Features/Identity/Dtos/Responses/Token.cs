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

        public Token(string accessToken)
        {
            AccessToken = accessToken;
           
        }
    }
}
