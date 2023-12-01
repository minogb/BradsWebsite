using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace BradsWebsite.Models
{
    public class HomeDataModel
    {
        public int PostCount { get; set; }

        public HomeDataModel(IConfiguration configuration) {//todo
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
                            int tmp;
                            if(int.TryParse(reader[0].ToString(), out tmp))
                            {
                                PostCount = tmp;
                            }
                        }

                    }
                }
            }
        }
    }
}
