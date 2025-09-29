using MetaBlog.Domain.Comments;
using MetaBlog.Domain.Common;
using MetaBlog.Domain.Favorites;
using MetaBlog.Domain.Likes;
using MetaBlog.Domain.Posts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MetaBlog.Domain.Users
{
    public class User:AuditableEntity
    {
        public string? Bio { get; private set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? imageUrl { get; private set; }
        public bool? isActive { get; private set; }

        private readonly List<Post> _Posts = new List<Post>();
      
        public IEnumerable<Post> Posts => _Posts.AsReadOnly();

        private readonly List<Like> _Likes = new List<Like>();
    
        public IEnumerable<Like> Likes => _Likes.AsReadOnly();

        private readonly List<Comment> _Comments = new List<Comment>();
  
        public IEnumerable<Comment> Comments => _Comments.AsReadOnly();

        private readonly List<Favorite> _Favorites = new List<Favorite>();

        public IEnumerable<Favorite> Favorites => _Favorites.AsReadOnly();

        private User() { }
        private User(Guid id,DateOnly dob):base(id)
        {
            DateOfBirth = dob;
        }

        public static User Create(Guid id,DateOnly dob)
        {
            return new User(id,dob);
        }
        public void Update(string? bio,string? imageUrl)
        {
            Bio = bio;
            this.imageUrl = imageUrl;
        }
        public void Activate()
        {
            isActive = true;
        }

    }
}
