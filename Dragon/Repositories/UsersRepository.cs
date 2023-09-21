using Dragon.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Dragon.Repositories
{

    public class UsersRepository
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



        public IList<Users> GetAllUsers()
        {
            IList<Users> users = new List<Users>();
            using (var conn = new SqlConnection(connectionString))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT n.Id, 
                                           n.Email, 
                                           n.Username, 
                                           a.[RoleId] 
                                    FROM dbo.AspNetUsers as n 
                                    LEFT JOIN dbo.AspNetUserRoles as a on (n.Id=a.UserId) 
                                    ";
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Users user = new Users()
                            {
                                UserID = reader.GetString(reader.GetOrdinal("Id")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                UserName = reader.GetString(reader.GetOrdinal("Username")),
                                RoleId = reader.GetString(reader.GetOrdinal("RoleId"))
                            };
                            users.Add(user);
                        }
                    }
                }

            }
            return users;

        }

        public Users GetById(string id)
        {
            Users usr = null;
            using(var conn = new SqlConnection(connectionString))
            {
                using(var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT n.Id, 
                                           n.Email, 
                                           n.Username, 
                                           r.[Name] as 'Role' 
                                    FROM dbo.AspNetUsers as n 
                                    LEFT JOIN dbo.AspNetUserRoles as a on (n.Id=a.UserId) 
                                    left join dbo.AspNetRoles r on a.RoleId=r.Id where n.Id=@UserID";
                    cmd.Parameters.AddWithValue("UserID", id);
                    conn.Open();
                    using(var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usr = new Users()
                            {
                                UserID = reader.GetString(0),
                                Email = reader.GetString(1),
                                UserName = reader.GetString(2),
                                RoleId = reader.GetString(3)
                            };

                        }
                    }
                }
            }
            return usr;
            
        }
        public void Delete(string id)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM [dbo].[AspNetUsers] WHERE Id=@UserID";
                    cmd.Parameters.AddWithValue("@UserID", id);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

        }
        public void Edit(Users usr)
        {
            using(DbConnection connection = new SqlConnection(connectionString))
            {
                using(DbCommand cmd=connection.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE [dbo].[AspNetUsers]
                                        SET [Email] = @Email
                                           ,[UserName] = @UserName
                                        WHERE [Id]=@UserId;
                                        UPDATE [dbo].[AspNetUserRoles]
                                          SET [RoleId] = @RoleId
                                        WHERE [UserId] = @UserId";

                    
                    DbParameter UserId = cmd.CreateParameter();
                    UserId.ParameterName = "@UserId";
                    UserId.Value =  usr.UserID;
                    UserId.DbType = System.Data.DbType.String;
                    cmd.Parameters.Add(UserId);

                    DbParameter RoleId = cmd.CreateParameter();
                    RoleId.ParameterName = "@RoleId";
                    RoleId.Value = usr.RoleId;
                    RoleId.DbType = System.Data.DbType.String;
                    cmd.Parameters.Add(RoleId);
                    


                    
                    DbParameter Email = cmd.CreateParameter();
                    Email.ParameterName = "@Email";
                    Email.Value = usr.Email;
                    Email.DbType = System.Data.DbType.String;
                    cmd.Parameters.Add(Email);

                    DbParameter Username = cmd.CreateParameter();
                    Username.ParameterName = "@UserName";
                    Username.Value = usr.UserName;
                    Username.DbType = System.Data.DbType.String;
                    cmd.Parameters.Add(Username);
                    


                    /*DbParameter UserId = cmd.CreateParameter();
                    UserId.ParameterName = "@UserId";
                    UserId.Value = usr.UserID;
                    UserId.DbType = System.Data.DbType.Int32;
                    cmd.Parameters.Add(UserId);

                    cmd.Parameters.AddWithValue("@Username",usr.UserName);*/


                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        
        public void EditDetails(Users usr)
        {
            using(DbConnection connection = new SqlConnection(connectionString))
            {
                using(DbCommand cmd=new SqlCommand())
                {
                    cmd.CommandText = @"UPDATE [dbo].[AspNetUsers]
                                        SET [UserName] = @Username,
                                            [Email]=@Email
                                        WHERE [Id]=@Id
                                        ";

                    
                    
                    
                    DbParameter UserId = cmd.CreateParameter();
                    UserId.ParameterName = "@Id";
                    UserId.Value =  usr.UserID;
                    UserId.DbType = System.Data.DbType.String;
                    cmd.Parameters.Add(UserId);



                    DbParameter Email = cmd.CreateParameter();
                    Email.ParameterName = "@Email";
                    Email.Value = usr.Email;
                    Email.DbType = System.Data.DbType.String;
                    cmd.Parameters.Add(Email);

                    DbParameter Username = cmd.CreateParameter();
                    Username.ParameterName = "@UserName";
                    Username.Value = usr.UserName;
                    Username.DbType = System.Data.DbType.String;
                    cmd.Parameters.Add(Username);
                    


                    /*DbParameter UserId = cmd.CreateParameter();
                    UserId.ParameterName = "@UserId";
                    UserId.Value = usr.UserID;
                    UserId.DbType = System.Data.DbType.Int32;
                    cmd.Parameters.Add(UserId);

                    cmd.Parameters.AddWithValue("@Username",usr.UserName);*/


                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public IList<Role> GetAllRoles()
        {
            IList<Role> roles = new List<Role>();
            using (var conn = new SqlConnection(connectionString))
            {
                using(var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT [Id], [Name] from [dbo].[AspNetRoles]";
                    conn.Open();
                    using(var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Role role = new Role()
                            {
                                Id=reader.GetInt32(0),
                                Name=reader.GetString(1)
                            };
                            roles.Add(role);
                        }
                    }
                }
            }
            return roles;
        }
    }
}
    