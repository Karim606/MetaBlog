using MetaBlog.Domain.Likes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Likes.Dtos
{
    public class LikeDto
    {
        public Guid Id { get; set; }
        public Guid TargetId { get; set; }
        public LikeTargetType TargetType { get; set; }
        public Guid UserId { get; set; }
        public DateTime LikedAt { get; set; }
        public string UserName { get; set; }
        public string? imageUrl { get; set; }
        public bool followed { get; set; }
    }
}
