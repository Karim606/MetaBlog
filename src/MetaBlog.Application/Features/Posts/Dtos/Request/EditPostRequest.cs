using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Posts.Dtos.Request
{
    public class EditPostRequest
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
