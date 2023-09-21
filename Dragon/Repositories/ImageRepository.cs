using Dragon.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Dragon.Repositories
{
    public class ImageRepository
    {
        //gets by id of the image
        public Image GetById(int id)
        {
            Image image = null;
            using (var conn = new SqlConnection(connectionString))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT [ImageID]
      ,[SupplierID]
      ,[ImageContent]
  FROM [dbo].[Image] WHERE ImageID=@ImageID ";
                    cmd.Parameters.AddWithValue("@ImageID", id);
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            image = new Image()
                            {
                                ImageID = id,
                                SupplierID = reader.GetInt32(reader.GetOrdinal("SupplierID")),
                                ImageContent = reader.IsDBNull(reader.GetOrdinal("ImageContent")) ? null : (byte[])reader["ImageContent"]

                            };
                        }
                    }
                }
            }

            return image;
        }
        //Gets the last uploaded photo for supplier
        public Image GetBySupplierID(int id)
        {
            //IList<Image> imageList = new List<Image>();
            Image image = null;
            using (var conn = new SqlConnection(connectionString))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT [ImageID]
                                            ,[SupplierID]
                                            ,[ImageContent]
                                        FROM [dbo].[Image] WHERE SupplierID=@SupplierID ";
                    cmd.Parameters.AddWithValue("@SupplierID", id);
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            image = new Image()
                            {
                                ImageID = reader.GetInt32(reader.GetOrdinal("ImageID")),
                                SupplierID = id,
                                ImageContent = reader.IsDBNull(reader.GetOrdinal("ImageContent")) ? null : (byte[])reader["ImageContent"]

                            };




                        }
                    }
                }
            }





            return image;
        }
        //db connection string
        public static string connectionString
        {
            get
            {
                return WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            }
        }
        //creating a new image on db
        public void Create(Image image, bool backImg)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                using (var cmd = conn.CreateCommand())
                {

                    string sql1 = @"INSERT INTO [dbo].[Image]
                                             ([SupplierID]
                                             ,[ImageContent]
                                             )
                                       VALUES
                                             (@SupplierID
                                             ,@ImageContent
                                             ) ;
                                       ";
                    string sql2 = @"UPDATE [dbo].[Supplier]
                                        SET [BackgroundImageID]		= (SELECT TOP 1 ImageID FROM [dbo].[Image] ORDER BY ImageID DESC)	                                           				
                                       WHERE [SupplierID]				= @SupplierID";
                    
                    
                    cmd.Parameters.AddWithValue("@SupplierID", image.SupplierID);
                    cmd.Parameters.AddWithValue("@ImageContent", (object)image.ImageContent ?? SqlBinary.Null);
                   
                    
                    if (backImg.Equals(true))
                    {
                        cmd.CommandText = sql1+sql2;
                    }
                    else
                    {
                        cmd.CommandText = sql1;
                    }


                    conn.Open();
                    cmd.ExecuteNonQuery();
                    

                }
            }
        }
        //deleting photo from db
        public void Delete(int id)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM [dbo].[Image] WHERE ImageID=@ImageID";
                    cmd.Parameters.AddWithValue("@ImageID", id);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public IList<Image> GetImagesForSupplier(int id)
        {
            IList<Image> imageList = new List<Image>();
            using (var conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT [ImageID]
                                             ,[SupplierID]
                                             ,[ImageContent]
                                       FROM [dbo].[Image] WHERE SupplierID=@SupplierID ";
                    cmd.Parameters.AddWithValue("@SupplierID", id);

                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Image image = new Image()
                            {
                                ImageID = reader.GetInt32(reader.GetOrdinal("ImageID")),
                                SupplierID = reader.GetInt32(reader.GetOrdinal("SupplierID")),
                                ImageContent = reader.IsDBNull(reader.GetOrdinal("ImageContent")) ? null : (byte[])reader["ImageContent"]
                            };
                            imageList.Add(image);
                        }
                    }
                }
            }
            return imageList;
        }

        public void MakeImgBackground(int id, int supplierID)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE [dbo].[Supplier]
                                        SET [BackgroundImageID]		= @BackgroundImageID	                                           				
                                       WHERE [SupplierID]				= @SupplierID";
                    cmd.Parameters.AddWithValue("@BackgroundImageID", id);
                    cmd.Parameters.AddWithValue("@SupplierID", supplierID);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }


        public CategoryImage GetCategoryImage(int id)
        {
            CategoryImage catImg = null;
            using (var conn = new SqlConnection(connectionString))
            {
                using(var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT [ImageID]
      ,[CategoryID]
      ,[ImageContent]
  FROM [dbo].[ImageCategory] WHERE CategoryID=@CategoryID";
                    cmd.Parameters.AddWithValue("@CategoryID",id);
                    conn.Open();
                    using(var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            catImg = new CategoryImage()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("ImageID")),
                                CategoryId = id,
                                ImageContent = reader.IsDBNull(reader.GetOrdinal("ImageContent")) ? null : (byte[])reader["ImageContent"]
                            };
                        }
                    }
                }
            }
            return catImg;
        }

        public void CreateCategoryImage(CategoryImage categoryImage)
        {
            using(var conn = new SqlConnection(connectionString))
            {
                using(var command = conn.CreateCommand())
                {
                    command.CommandText = @"INSERT INTO [dbo].[ImageCategory]
           ([ImageID]
           ,[CategoryID]
           ,[ImageContent])
     VALUES
           (@ImageID
           ,@CategoryID
           ,@ImageContent)";
                    command.Parameters.AddWithValue("@ImageID",categoryImage.Id);
                    command.Parameters.AddWithValue("@CategoryID",categoryImage.CategoryId);
                    command.Parameters.AddWithValue("@ImageContent", (object)categoryImage.ImageContent ?? SqlBinary.Null);

                    conn.Open();
                    command.ExecuteNonQuery();

                }
            }
        }

        public void DeleteCategoryImage(int id)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM [dbo].[ImageCategory] WHERE ImageID=@ImageID";
                    cmd.Parameters.AddWithValue("@ImageID", id);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}