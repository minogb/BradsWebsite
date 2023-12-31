﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Data;
using System.Security.Claims;
using Microsoft.Data.SqlClient;
using System.Configuration;
using Microsoft.AspNetCore.Http;

namespace BradsWebsite.Authentication
{
    public class UserManager
    {
        IConfiguration Configuration { get; set; }

        public UserManager(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public bool SignUp(HttpContext httpContext, AuthUser user, bool isPersistent = false)
        {
            using (var con = new SqlConnection(Configuration.GetConnectionString("Primary")))
            {
                using (SqlCommand cmd = new SqlCommand("CreateUser", con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("UserEmail", user.Email);
                    cmd.Parameters.AddWithValue("UserName", user.Name);
                    cmd.Parameters.AddWithValue("UserSecrete", user.PasswordHashed());
                    var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Bit);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    if ((int)returnParameter.Value == 0)
                    {
                        return false;
                    }
                }
                return SignIn(httpContext, user, isPersistent);
            }
        }
        public bool SignIn(HttpContext httpContext, AuthUser user, bool isPersistent = false)
        {
            using (var con = new SqlConnection(Configuration.GetConnectionString("Primary")))
            {
                using (SqlCommand cmd = new SqlCommand("GetUser", con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("UserEmail", user.Email);
                    cmd.Parameters.AddWithValue("UserSecrete", user.PasswordHashed());
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        if (reader.Read())
                        {
                            user.Id = reader["Id"].ToString().Trim();
                            user.Name = reader["Name"].ToString().Trim();
                            user.Locked = reader["Locked"] as DateTime?;
                            user.Disabled = reader["Disabled"] as DateTime?;
                            user.AccountType = reader["AccountType"].ToString().Trim();
                            user.Confirmed = reader["Disabled"] as DateTime?;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                if(user.Confirmed == null)
                {
                   //todo
                   //return false;
                }
                ClaimsIdentity identity = new ClaimsIdentity(this.GetUserClaims(user), CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal).Wait();
                return true;
            }
        }
        public List<string> GetUserRoles(AuthUser user)
        {
            var roles = new List<string>();//todo\

            using (var con = new SqlConnection(Configuration.GetConnectionString("Primary")))
            {
                using (SqlCommand cmd = new SqlCommand("GetUserRoles", con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("UserId", user.Id);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while(reader.Read()) { 
                            roles.Add(reader.GetString(0));
                        }
                    }
                }
            }
            return roles;
        }

        public async void SignOut(HttpContext httpContext)
        {
            await httpContext.SignOutAsync();
        }

        private IEnumerable<Claim> GetUserClaims(AuthUser user)
        {
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            claims.Add(new Claim(ClaimTypes.Name, user.Name));
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            claims.AddRange(this.GetUserRoleClaims(user));
            return claims;
        }

        private IEnumerable<Claim> GetUserRoleClaims(AuthUser user)
        {
            List<Claim> claims = new List<Claim>();
            foreach (var role in GetUserRoles(user))
                claims.Add(new Claim(ClaimTypes.Role, role));
            return claims;
        }
    }
}
