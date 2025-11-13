using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Identity.Dtos.Requests
{
    public class ResetPasswordDto
    {
        public string email { get; set; }
        public string token { get; set; }
        public string newPassword { get; set; }

    }
}
