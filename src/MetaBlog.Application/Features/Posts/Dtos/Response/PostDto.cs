using MetaBlog.Domain.Comments;
using MetaBlog.Domain.Likes;
using MetaBlog.Domain.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Posts.Dtos.Response
{
    public class PostDto
    {
        public Guid Id { get; set; }
        public string Title { get;  set; }
        public string Content { get;  set; }
        public string Slug { get;  set; }
        public string  UserName { get;  set; }
        public int LikesCount { get;  set; }
        public int CommentsCount { get;  set; }
        




    }
}
