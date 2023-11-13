using Microsoft.AspNetCore.Mvc;

namespace BradsWebsite.Areas.Post.Models
{
    public class PostPageModel
    {
        public int? ParentID { get; set; }
        public PostModel Parent { get; set; }
        public string Tag { get; set; }
        public string Mention { get; set; }

        public DateTime? From { get; set; }
    }
}
