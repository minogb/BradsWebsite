using Microsoft.Data.SqlClient;
using Microsoft.DiaSymReader;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Reflection.Metadata;

namespace BradsWebsite.Models
{
    public class DailyMessageModel
    {
        public int? Id { get; set; }
        [Required]
        public string Message { get; set; }
        public string? Category { get; set; }
        [DataType(DataType.Date)]
        public DateTime? Start {  get; set; }
        [DataType(DataType.Date)]
        public DateTime? End { get; set; }
        public static void Delete(int id, IConfiguration configuration)
        {
            using (var con = new SqlConnection(configuration.GetConnectionString("Primary")))
            {
                using (SqlCommand cmd = new SqlCommand("DeleteDailyMessage", con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("Id", id);
                    cmd.CommandType = CommandType.StoredProcedure;
                    var reader = cmd.ExecuteNonQuery();
                }
            }
        }
        public DailyMessageModel() {
        }
        public DailyMessageModel(int id, IConfiguration configuration)
        {
            using (var con = new SqlConnection(configuration.GetConnectionString("Primary")))
            {
                using (SqlCommand cmd = new SqlCommand("GetDailyMessage", con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Id = reader.GetInt32(0);
                            Message = reader.GetString(1);
                            if (reader[2] != null && !reader.IsDBNull(2))
                                Start = GetDateOnly(reader.GetString(2));
                            if (reader[3] != null && !reader.IsDBNull(2))
                                End = GetDateOnly(reader.GetString(3));
                            Category = reader.GetString(4);
                            Category = char.ToUpper(Category[0]) + Category.Substring(1).ToLower();
                        }
                    }
                }
            }
        }
        public DailyMessageModel(string category, IConfiguration configuration)
        {
            Category = category;
            Category = char.ToUpper(Category[0]) + Category.Substring(1).ToLower();
            using (var con = new SqlConnection(configuration.GetConnectionString("Primary")))
            {
                using (SqlCommand cmd = new SqlCommand("GetDailyMessage", con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("Category", category);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Id = reader.GetInt32(0);
                            Message = reader.GetString(1);
                            if (reader[2] != null && !reader.IsDBNull(2))
                                Start = GetDateOnly(reader.GetString(2));
                            if (reader[3] != null && !reader.IsDBNull(3))
                                End = GetDateOnly(reader.GetString(3));
                        }
                    }
                }
            }
        }
        public bool SaveEdit(IConfiguration configuration)
        {
            using (var con = new SqlConnection(configuration.GetConnectionString("Primary")))
            {
                using (SqlCommand cmd = new SqlCommand("EditDailyMessage", con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("Id", Id);
                    cmd.Parameters.AddWithValue("Message", Message);
                    cmd.Parameters.AddWithValue("Category", Category);
                    if (Start.HasValue && End.HasValue)
                    {
                        cmd.Parameters.AddWithValue("Start", Start.Value.ToString("MM-dd-yyyy"));
                        cmd.Parameters.AddWithValue("End", End.Value.ToString("MM-dd-yyyy"));
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            return Id > 0;
        }
        public bool SaveNew(IConfiguration configuration)
        {
            using (var con = new SqlConnection(configuration.GetConnectionString("Primary")))
            {
                using (SqlCommand cmd = new SqlCommand("CreateDailyMessage", con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("Message", Message);
                    cmd.Parameters.AddWithValue("Category", Category);
                    if (Start.HasValue && End.HasValue)
                    {
                        cmd.Parameters.AddWithValue("Start", Start.Value.ToString("MM-dd-yyyy"));
                        cmd.Parameters.AddWithValue("End", End.Value.ToString("MM-dd-yyyy"));
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Id = (int)reader.GetDecimal(0);
                        }
                    }
                }
            }
            return Id > 0;
        }
        private DateTime? GetDateOnly(string value)
        {
            try
            {
                if (!string.IsNullOrEmpty(value))
                {
                    var se = value.Split("-");
                    if (se.Length > 1)
                    {
                        int m, d;
                        if (int.TryParse(se[0], out m) && int.TryParse(se[1], out d))
                        {
                            return new DateTime(2024, m, d);
                        }
                    }
                }
            }
            catch
            {

            }
            return null;
        }
    }
}
