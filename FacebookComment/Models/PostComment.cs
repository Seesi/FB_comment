using System.ComponentModel.DataAnnotations;

namespace FacebookComment.Models
{
    public class PostComment
    {
        [Key]
        public int CommentId { get; set; }
        public int PostId { get; set; }
        public string Message { get; set; }
        public string CommentedBy { get; set; }
        public System.DateTime CommentedDate { get; set; }

        public virtual Post Post { get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }
}