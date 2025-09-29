using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Comments.Dtos
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public Guid postId { get; set; }
        public Guid userId { get; set; }
        public Guid? parentCommentId { get; set; }
        public DateTime createdAt { get; set; }
        public string userName { get; set; }
        public string Content { get; set; }
        public int likesCount { get; set; }
        public int repliesCount { get; set; }
        
    }
}
