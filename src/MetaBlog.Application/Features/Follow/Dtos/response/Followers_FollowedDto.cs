using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Follow.Dtos.response
{
    public class Followers_FollowedDto
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string imageUrl { get; set; }

    }
}
