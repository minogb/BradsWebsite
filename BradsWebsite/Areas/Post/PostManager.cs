using ProfanityFilter;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Claims;
using BradsWebsite.Areas.Post.Models;
using Microsoft.Extensions.Hosting;
using NuGet.Packaging;
using System.Configuration;

namespace BradsWebsite.Areas.Post
{
    public class PostManager
    {
        IConfiguration Configuration;

        public PostManager(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public bool CreatePost(HttpContext context, CreatePostModel post)
        {
            using (var con = new SqlConnection(Configuration.GetConnectionString("Primary")))
            {
                using (SqlCommand cmd = new SqlCommand("CreatePost", con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("UserID", context.User.FindFirstValue(ClaimTypes.NameIdentifier));
                    cmd.Parameters.AddWithValue("Message", post.Message);
                    cmd.Parameters.AddWithValue("ParentId", post.ReplyTo);
                    var dt = new DataTable();
                    dt.Columns.Add("RowID");
                    dt.Columns.Add("Link");
                    List<string> links = new List<string>();
                    if (post.Links != null)
                    {
                        foreach (var link in post.Links)
                        {
                            if (link == null || link.Url == null)
                                continue;
                            string l = link.Url.Trim();
                            if (!string.IsNullOrEmpty(l))
                            {
                                dt.Rows.Add(dt.Rows.Count, link.Url);
                            }
                        }
                    }
                    cmd.Parameters.AddWithValue("Links", dt);
                    var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Bit);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    if ((int)returnParameter.Value == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public List<string> GetPostLinks(int postId)
        {
            var retVar = new List<string>();
            using (var con = new SqlConnection(Configuration.GetConnectionString("Primary")))
            {
                using (SqlCommand cmd = new SqlCommand("GetPostLinks", con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("PostId", postId);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            retVar.Add(reader.GetString(0));
                        }
                    }
                }
            }
            return retVar;
        }
        public List<PostModel> GetPosts(HttpContext context, PostPageModel page)
        {
            var filter = new ProfanityFilter.ProfanityFilter();

            var retVar = new List<PostModel>();
            using (var con = new SqlConnection(Configuration.GetConnectionString("Primary")))
            {
                using (SqlCommand cmd = new SqlCommand("GetPostPage", con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("From", DateTime.Now);
                    string value = page.Tag;
                    if(value != null)
                        value = value.Trim();
                    cmd.Parameters.AddWithValue("Tag", value);
                    value = page.Mention;
                    if (value != null)
                        value = value.Trim();
                    cmd.Parameters.AddWithValue("Mention", value);//todo other stuff here
                    if (page.ParentID != null)
                    {
                        cmd.Parameters.AddWithValue("Parent", page.ParentID);
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read()) {
                            var post = new PostModel();
                            post.User = new PostUserModel();
                            post.User.Id = reader.GetInt32(0);
                            post.User.UserName = reader.GetString(1).Trim();
                            post.Id = reader.GetInt32(2);
                            post.Message = filter.CensorString(reader.GetString(3)).Trim();
                            post.Date = reader.GetDateTime(4);
                            post.Replies = reader.GetInt32(5);
                            if (!reader.IsDBNull(6))
                                post.ParentPost = reader.GetInt32(6);
                            if (post.Id == page.ParentID)
                            {
                                page.Parent = post;
                            }
                            else
                            {
                                retVar.Add(post);
                            }
                            post.Links = GetPostLinks(post.Id).ToArray();
                        }
                    }
                }
            }
            return retVar;
        }
        public bool DeletePost(HttpContext context, int postId)
        {
            using (var con = new SqlConnection(Configuration.GetConnectionString("Primary")))
            {
                using (SqlCommand cmd = new SqlCommand("DeletePost", con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("UserID", context.User.FindFirstValue(ClaimTypes.NameIdentifier));
                    cmd.Parameters.AddWithValue("PostID", postId);
                    var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Bit);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    if ((int)returnParameter.Value == 0)
                    {
                        return false;
                    }
                }
            }
            return false;
        }
        public List<PostModel> GetReplies(HttpContext context, int postId)
        {
            //todo
            return new List<PostModel>();
        }
        public PostModel GetPost(HttpContext context, int postId)
        {
            //todo
            return new PostModel();
        }
        public void GetFullPost(HttpContext context, int postId, out PostModel post, out List<PostModel> replies)
        {
            post = GetPost(context, postId);
            replies = GetReplies(context, postId);

        }
    }
}
