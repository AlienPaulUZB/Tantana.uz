using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Dragon.Repositories
{
    public class AccountRepository
    {
        public static string connectionString
        {
            get
            {
                return WebConfigurationManager
                    .ConnectionStrings["DefaultConnection"]
                    .ConnectionString;

            }
        }

        public void AssignDefaultRole(string id)
        {
            using(var conn=new SqlConnection(connectionString))
            {
                using(var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO [dbo].[AspNetUserRoles]
                                              ([UserId]
                                              ,[RoleId])
                                        VALUES
                                              (@UserId
                                              ,@RoleId)";
                    cmd.Parameters.AddWithValue("@UserId", id);
                    cmd.Parameters.AddWithValue("@RoleId", 3);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        
    }
}