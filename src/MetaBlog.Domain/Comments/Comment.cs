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

namespace MetaBlog.Domain.Comments
{
    public class Comment:AuditableEntity,ILikable
    {
        public Guid postId { get; }
        public Guid userId { get; }
        public string content { get; private set; }
        public Guid? parentCommentId { get; }
        public User User { get; private set; }

        private readonly List<Comment> _Replies = new List<Comment>();
       
        public IEnumerable<Comment> Replies => _Replies.AsReadOnly();

        public int likesCount { get; private set; }

        private Comment() { }
        private Comment(Guid id,Guid postId,Guid userId,string content,Guid? parentCommentId):base(id)
        {
            this.postId = postId;
            this.userId = userId;
            this.content = content;
            this.parentCommentId = parentCommentId;
        }
        public static Comment Create(Guid id,Guid postId,Guid userId,string content,Guid? parentCommentId)
        {
            return new Comment(id,postId,userId,content,parentCommentId);
        }
        public  void UpdateContent(string content)
        {
            this.content = content;
        }

        public void IncrementLike() => likesCount++;


        public void DecrementLike() => likesCount = likesCount > 0 ? likesCount-- : likesCount;
    }
}
