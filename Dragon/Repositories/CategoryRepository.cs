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
    public class CategoryRepository
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

        public IList<Category> GetAll()
        {
            IList<Category> categories = new List<Category>();
            using (DbConnection con = new SqlConnection(connectionString))
            {
                using (DbCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SELECT [CategoryID],[CategoryName],[CategoryDescription] FROM [dbo].[Category]";
                    con.Open();
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Category c = new Category()
                            {
                                CategoryID = reader.GetInt32(0),
                                CategoryName = reader.GetString(1),
                                CategoryDescription = reader.GetString(2)
                            };
                            categories.Add(c);
                        }
                    }
                }
            }
            return categories;
        }

        public void Create(Category category)
        {
            using (DbConnection con = new SqlConnection(connectionString))
            {
                using (DbCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO [dbo].[Category]
                                                ([CategoryName]
                                                ,[CategoryDescription])
                                          VALUES
                                                (@CategoryName
                                                ,@CategoryDescription)";
                    DbParameter pCategoryName = cmd.CreateParameter();
                    pCategoryName.Value = category.CategoryName;
                    pCategoryName.ParameterName = "@CategoryName";
                    pCategoryName.DbType = System.Data.DbType.String;
                    cmd.Parameters.Add(pCategoryName);

                    DbParameter pCategoryDesc = cmd.CreateParameter();
                    pCategoryDesc.Value = category.CategoryDescription;
                    pCategoryDesc.ParameterName = "@CategoryDescription";
                    pCategoryDesc.DbType = System.Data.DbType.String;
                    cmd.Parameters.Add(pCategoryDesc);

                    con.Open();
                    cmd.ExecuteNonQuery();


                }
            }
        }

        public void Update(Category category)
        {
            using (DbConnection connection = new SqlConnection(connectionString))
            {
                using (DbCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE [dbo].[Category]
                                        SET [CategoryName]			= @CategoryName
                                            ,[CategoryDescription]	= @CategoryDescription
                                        WHERE [CategoryID]=@CategoryID";
                    DbParameter pCategoryID = cmd.CreateParameter();
                    pCategoryID.ParameterName = "@CategoryID";
                    pCategoryID.Value = category.CategoryID;
                    pCategoryID.DbType = System.Data.DbType.Int32;
                    cmd.Parameters.Add(pCategoryID);


                    DbParameter pCategoryName = cmd.CreateParameter();
                    pCategoryName.Value = category.CategoryName;
                    pCategoryName.ParameterName = "@CategoryName";
                    pCategoryName.DbType = System.Data.DbType.String;
                    cmd.Parameters.Add(pCategoryName);

                    DbParameter pCategoryDesc = cmd.CreateParameter();
                    pCategoryDesc.Value = category.CategoryDescription;
                    pCategoryDesc.ParameterName = "@CategoryDescription";
                    pCategoryDesc.DbType = System.Data.DbType.String;
                    cmd.Parameters.Add(pCategoryDesc);

                    connection.Open();
                    cmd.ExecuteNonQuery();



                }
            }
        }


        public Category GetById(int id)
        {
            Category category = null;
            using (var connection = new SqlConnection(connectionString))
            {
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = @"SELECT [CategoryID]
                                            ,[CategoryName]
                                            ,[CategoryDescription]
                                        FROM [dbo].[Category]
                                        WHERE [CategoryID]=@CategoryId";
                    cmd.Parameters.AddWithValue("CategoryID", id);
                    connection.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            category = new Category()
                            {
                                CategoryID = id,
                                CategoryName = reader.GetString(reader.GetOrdinal("CategoryName")),
                                CategoryDescription = reader.GetString(reader.GetOrdinal("CategoryDescription"))
                            };
                        }
                    }
                }
            }
            return category;
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM [dbo].[Category] WHERE CategoryID=@CategoryID";
                    command.Parameters.AddWithValue("CategoryID", id);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }


    }
}