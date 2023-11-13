using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace BradsWebsite.Areas.Post.Models
{
    public class CreatePostModel
    {
        [Required]
        [StringLength(int.MaxValue)]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }
        [MaxLength(5)]
        public List<PostLinkModel> Links { get; set; } = new List<PostLinkModel>();
        
        public int? ReplyTo { get; set; }
    }
    public class PostLinkModel
    {
        [DataType(DataType.Url)]
        [Url]
        public string Url { get { return _Url; } set
            {
                if(value != null)
                {
                    if (!(value.StartsWith("http://") || value.StartsWith("https://")))
                    {
                        value = "https://" + value;
                    }
                    _Url = value;
                }
            }
        }
        private string _Url { get; set; }
    }
}
