using Microsoft.Data.SqlClient;
using System.Data;

namespace BradsWebsite.Models
{
    public class DailyMessageCategoryModel
    {
        public string Category { get; set; }
        public List<DailyMessageModel> Messages { get; set; }
        public DailyMessageCategoryModel(string category, IConfiguration configuration)
        {
            this.Category = category;
            Messages = new List<DailyMessageModel>();
            using (var con = new SqlConnection(configuration.GetConnectionString("Primary")))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT [ID] FROM [DailyMessage] WHERE [Category] = UPPER(@Category) ", con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("Category", category);
                    cmd.CommandType = CommandType.Text;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            Messages.Add(new DailyMessageModel(reader.GetInt32(0), configuration));
                        }
                    }
                }
            }
        }
        public DailyMessageCategoryModel(IConfiguration configuration)
        {
            Messages = new List<DailyMessageModel>();
            using (var con = new SqlConnection(configuration.GetConnectionString("Primary")))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT [Category] FROM [DailyMessage]", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.Text;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Messages.Add(new DailyMessageModel(reader.GetString(0), configuration));
                        }
                    }
                }
            }
        }
    }
}
