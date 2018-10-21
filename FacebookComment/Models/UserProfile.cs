using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FacebookComment.Models
{
    public class UserProfile
    {
        public UserProfile()
        {
            this.PostComments = new HashSet<PostComment>();
            this.Posts = new HashSet<Post>();
        }
        [Key]

        public int UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string AvatarExt { get; set; }

        public virtual ICollection<PostComment> PostComments { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}