using MetaBlog.Domain.Comments;
using MetaBlog.Domain.Common;
using MetaBlog.Domain.Common.Interfaces;
using MetaBlog.Domain.Likes;
using MetaBlog.Domain.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Domain.Posts
{
    public class Post:AuditableEntity,ILikable
    {
        public string Title { get; private set; }
        public string Content { get; private set; }
        public string Slug { get; private set; }
        public Guid authorId { get; }

        public User User { get; private set; }


        private readonly List<Comment> _Comments = new List<Comment>();
      
        public IEnumerable<Comment> Comments => _Comments.AsReadOnly();

        public int likesCount {  get; private set; }

        private Post() { }
       private Post(Guid id,string title,string content,Guid authorId):base(id)
        {
            Title = title.TrimEnd();
            Content = content;
            Slug = title.TrimEnd().Replace(" ","-").ToLower();
            this.authorId = authorId;
            likesCount = 0;
        }
        public static Post Create(Guid id,string title,string content,Guid authorId)
        {
            return new Post(id,title,content,authorId);
        }
        public void Update(string title,string content)
        {
            Title = title.TrimEnd();
            Content = content;
            Slug = title.TrimEnd().Replace(" ", "-").ToLower();
        }

        public void IncrementLike() => likesCount++;
        

        public void DecrementLike() => likesCount = likesCount >0?likesCount--:likesCount;
       
    }
}
