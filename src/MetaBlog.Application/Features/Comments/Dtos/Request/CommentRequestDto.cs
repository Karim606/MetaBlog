using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Comments.Dtos.Request
{
    public class CommentRequestDto
    {
        public string content { get; set; }
        public Guid? parentCommentId {  get; set; }
    }
}
