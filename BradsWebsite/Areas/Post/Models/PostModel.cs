using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace BradsWebsite.Areas.Post.Models
{
    public class PostModel
    {
        public int Id { get; set; }
        public int? ParentPost { get; set; }
        public PostUserModel User { get; set; } = new PostUserModel();
        [Required]
        [StringLength(int.MaxValue, MinimumLength = 1)]
        public string Message {get ; set;}
        public int Replies { get; set; }
        public string[] Links { get; set; } = new string[] { };
        public DateTime Date { get; set; }

    }
}
