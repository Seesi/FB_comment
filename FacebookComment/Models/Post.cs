using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FacebookComment.Models
{
    public class Post
    {
        public Post()
        {
            this.PostComments = new HashSet<PostComment>();
        }
        [Key]
        public int PostId { get; set; }
        public string Message { get; set; }
        public string PostedBy { get; set; }
        public System.DateTime PostedDate { get; set; }

        public virtual ICollection<PostComment> PostComments { get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }
}