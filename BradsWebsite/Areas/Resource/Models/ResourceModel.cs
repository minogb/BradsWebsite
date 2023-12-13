using Microsoft.Data.SqlClient;
using System.Data;

namespace BradsWebsite.Areas.Resource.Models
{
    public class ResourceModel
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public byte[] Data { get; set; }
        public ResourceModel(int id, IConfiguration configuration)
        {
            Id = id;
            using (var con = new SqlConnection(configuration.GetConnectionString("Primary")))
            {
                using (SqlCommand cmd = new SqlCommand("select count(*) From [Post].Post LEFT JOIN [User] ON [User].Id = [Post].UserID Where [User].Locked IS NULL AND [User].Disabled IS NULL AND [Post].DELETED IS NULL", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.Text;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Type = reader.GetString(0);
                            Data = (byte[])reader.GetValue(1);
                        }
                    }
                }
            }
        }
    }
}
