using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Users.Dtos
{
    public class UserProfileDto
    {
        public Guid id { get; set; }
        public string imageUrl { get; set; } = string.Empty;
        public string bio { get; set; } = string.Empty;
        public string userName { get; set; } = string.Empty;
    }
}
