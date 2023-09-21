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
    public class SupplierRepository
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


        public IList<Supplier> GetAll()
        {
            IList<Supplier> suppliersList = new List<Supplier>();
            using (DbConnection connection = new SqlConnection(connectionString))
            {
                using (DbCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = @"SELECT  s.[SupplierID] 
                                                ,s.[Name]
                                                ,s.[Description]
                                                ,s.[OwnerId]
                                                ,s.[Phone]
                                                /******,c.[CategoryName]*******/
                                                ,s.[Capacity]
                                                ,s.[Address]
                                                ,s.[Cost]
                                                ,s.[SocialMedia]
                                                ,s.[VideoLink]
                                                ,s.[BackgroundImageID]
                                                ,s.[City]
                                                ,s.[Brand]
                                                ,s.[Location]
                                        FROM [dbo].[Supplier] s left join dbo.Category c on c.CategoryID=s.CategoryID";
                    connection.Open();


                    using (DbDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Supplier supplier = new Supplier()
                            {
                                SupplierID = rdr.GetInt32(rdr.GetOrdinal("SupplierID")),
                                Name = rdr.GetString(rdr.GetOrdinal("Name")),
                                Description = rdr.GetString(rdr.GetOrdinal("Description")),
                                //OwnerID = rdr.GetInt32(rdr.GetOrdinal("OwnerID")),
                                Phone = rdr.GetString(rdr.GetOrdinal("Phone")),
                                //CategoryID          =rdr.GetInt32(rdr.GetOrdinal("CategoryID")),
                                Capacity = rdr.GetInt32(rdr.GetOrdinal("Capacity")),
                                Address = rdr.GetString(rdr.GetOrdinal("Address")),
                                Cost = rdr.GetDecimal(rdr.GetOrdinal("Cost")),
                                SocialMedia = rdr.GetString(rdr.GetOrdinal("SocialMedia")),
                                VideoLink = rdr.GetString(rdr.GetOrdinal("VideoLink")),
                                //BackgroundImageID   =rdr.IsDBNull(rdr.GetOrdinal("BackgroundImageID"))? 0 :(rdr.GetOrdinal("BackgroundImageID")),
                                City = rdr.GetString(rdr.GetOrdinal("City")),
                                Brand = rdr.GetString(rdr.GetOrdinal("Brand")),
                                Location = rdr.GetString(rdr.GetOrdinal("Location"))
                            };

                            suppliersList.Add(supplier);
                        }
                    }
                }
            }
            return suppliersList;
            Console.WriteLine(suppliersList);
        }
        public IList<Supplier> GetByCategory(int id)
        {
            IList<Supplier> suppliersList = new List<Supplier>();
            using (var connection = new SqlConnection(connectionString))
            {

                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = @"SELECT  s.[SupplierID] 
                                                ,s.[Name]
                                                ,s.[Description]
                                                ,s.[OwnerId]
                                                ,s.[Phone]
                                                ,s.[CategoryID]
                                                ,s.[Capacity]
                                                ,s.[Address]
                                                ,s.[Cost]
                                                ,s.[SocialMedia]
                                                ,s.[VideoLink]
                                                ,s.[BackgroundImageID]
                                                ,s.[City]
                                                ,s.[Brand]
                                                ,s.[Location]
                                        FROM [dbo].[Supplier] s left join dbo.Category c on c.CategoryID=s.CategoryID WHERE c.CategoryID=@CategoryID";
                    cmd.Parameters.AddWithValue("@CategoryID", id);

                    connection.Open();


                    using (DbDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Supplier supplier = new Supplier()
                            {
                                SupplierID = rdr.GetInt32(rdr.GetOrdinal("SupplierID")),
                                Name = rdr.GetString(rdr.GetOrdinal("Name")),
                                Description = rdr.GetString(rdr.GetOrdinal("Description")),
                                OwnerID = rdr.GetString(rdr.GetOrdinal("OwnerID")),
                                Phone = rdr.GetString(rdr.GetOrdinal("Phone")),
                                CategoryID = rdr.GetInt32(rdr.GetOrdinal("CategoryID")),
                                Capacity = rdr.GetInt32(rdr.GetOrdinal("Capacity")),
                                Address = rdr.GetString(rdr.GetOrdinal("Address")),
                                Cost = rdr.GetDecimal(rdr.GetOrdinal("Cost")),
                                SocialMedia = rdr.GetString(rdr.GetOrdinal("SocialMedia")),
                                VideoLink = rdr.GetString(rdr.GetOrdinal("VideoLink")),
                                BackgroundImageID   =rdr.IsDBNull(rdr.GetOrdinal("BackgroundImageID"))? 0 :(rdr.GetOrdinal("BackgroundImageID")),
                                City = rdr.GetString(rdr.GetOrdinal("City")),
                                Brand = rdr.GetString(rdr.GetOrdinal("Brand")),
                                Location = rdr.GetString(rdr.GetOrdinal("Location"))
                            };

                            suppliersList.Add(supplier);
                        }
                    }
                }
            }
            return suppliersList;
        }

        public IList<Supplier> Search(string supplierName, string cityFilter, int page,int pageSize,out int totalCount)
        {
            IList<Supplier> suppliersList = new List<Supplier>();
            using (var connection = new SqlConnection(connectionString))
            {
                using (var cmd = connection.CreateCommand())
                {
                    string fields = @"              [SupplierID]
                                                ,[Name]
                                                ,[Description]
                                                ,[OwnerId]
                                                ,[Phone]
                                                ,[CategoryID]
                                                ,[Capacity]
                                                ,[Address]
                                                ,[Cost]
                                                ,[SocialMedia]
                                                ,[VideoLink]
                                                ,[BackgroundImageID]
                                                ,[City]
                                                ,[Brand]
                                                ,[Location]";
                    string sql = @"SELECT   {0}
                                        FROM [dbo].[Supplier]";
                    
                    string whereSql = "";
                    if (!string.IsNullOrEmpty(supplierName))
                    {
                        whereSql += (whereSql.Length == 0 ? "" : " AND ")
                            + " [Name] like '%' + @SupplierName + '%' ";
                        cmd.Parameters.AddWithValue("@SupplierName", supplierName);
                    }

                    //search by city

                    if (!string.IsNullOrEmpty(cityFilter))
                    {
                        whereSql += (whereSql.Length == 0 ? "" : " AND ")
                            + " [City] like '%' + @CityFilter + '%' ";
                        cmd.Parameters.AddWithValue("@CityFilter", cityFilter);
                    }


                    if (!string.IsNullOrEmpty(whereSql))
                    {
                        whereSql = " WHERE " + whereSql;
                    }
                    
                    connection.Open();
                    cmd.CommandText=string.Format(sql, " count(*) ") + whereSql;
                    totalCount=(int)cmd.ExecuteScalar();

                    string pageSql = " OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY ";
                    cmd.Parameters.AddWithValue("@offset", (page - 1)*pageSize);
                    cmd.Parameters.AddWithValue("@pageSize",pageSize);
                    cmd.CommandText = string.Format(sql, fields) + whereSql + " ORDER BY [SupplierID]" + pageSql;


                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Supplier supplier = new Supplier()
                            {
                                SupplierID = rdr.GetInt32(rdr.GetOrdinal("SupplierID")),
                                Name = rdr.GetString(rdr.GetOrdinal("Name")),
                                Description = rdr.GetString(rdr.GetOrdinal("Description")),
                                OwnerID = rdr.GetString(rdr.GetOrdinal("OwnerID")),
                                Phone = rdr.GetString(rdr.GetOrdinal("Phone")),
                                CategoryID = rdr.GetInt32(rdr.GetOrdinal("CategoryID")),
                                Capacity = rdr.GetInt32(rdr.GetOrdinal("Capacity")),
                                Address = rdr.GetString(rdr.GetOrdinal("Address")),
                                Cost = rdr.GetDecimal(rdr.GetOrdinal("Cost")),
                                SocialMedia = rdr.GetString(rdr.GetOrdinal("SocialMedia")),
                                VideoLink = rdr.GetString(rdr.GetOrdinal("VideoLink")),
                                BackgroundImageID = rdr.GetInt32(rdr.GetOrdinal("BackgroundImageID")),
                                City = rdr.GetString(rdr.GetOrdinal("City")),
                                Brand = rdr.GetString(rdr.GetOrdinal("Brand")),
                                Location = rdr.GetString(rdr.GetOrdinal("Location"))

                            };
                            suppliersList.Add(supplier);
                        }
                    }

                }
            }
            return suppliersList;


        }
        //Search 2 function is used in ShowByCategory view for using search and filter functions 
        public IList<Supplier> Search2(int categoryID, string supplierName, string cityFilter, int page,int pageSize,out int totalCount, string orderByName)
        {
            IList<Supplier> suppliersList = new List<Supplier>();
            using (var connection = new SqlConnection(connectionString))
            {
                using (var cmd = connection.CreateCommand())
                {
                    string fields = @"              [SupplierID]
                                                ,[Name]
                                                ,[Description]
                                                ,[OwnerId]
                                                ,[Phone]
                                                ,[CategoryID]
                                                ,[Capacity]
                                                ,[Address]
                                                ,[Cost]
                                                ,[SocialMedia]
                                                ,[VideoLink]
                                                ,[BackgroundImageID]
                                                ,[City]
                                                ,[Brand]
                                                ,[Location]";
                    string sql = @"SELECT   {0}
                                        FROM [dbo].[Supplier]";
                    
                    string whereSql = "WHERE CategoryID=@CategoryID";
                    cmd.Parameters.AddWithValue("@CategoryID",categoryID);

                    //search by name
                    if (!string.IsNullOrEmpty(supplierName))
                    {
                        whereSql += (whereSql.Length == 0 ? "" : " AND ")
                            + " [Name] like '%' + @SupplierName + '%' ";
                        cmd.Parameters.AddWithValue("@SupplierName", supplierName);
                    }


                    //search by city

                    if (!string.IsNullOrEmpty(cityFilter))
                    {
                        whereSql += (whereSql.Length == 0 ? "" : " AND ")
                            + " [City] like '%' + @CityFilter + '%' ";
                        cmd.Parameters.AddWithValue("@CityFilter", cityFilter);
                    }

                    string orderSql = "";
                    //order by name
                    if (!string.IsNullOrEmpty(orderByName)&&orderByName=="ASC")
                    {
                        orderSql = " ORDER BY [Name] ASC";
                        
                    } else
                    {
                        orderSql = " ORDER BY [Name] DESC";
                    }
                    

                    /*
                    if (!string.IsNullOrEmpty(whereSql))
                    {
                        whereSql = " WHERE " + whereSql;
                    }
                    */
                    connection.Open();
                    cmd.CommandText=string.Format(sql, " count(*) ") + whereSql;
                    totalCount=(int)cmd.ExecuteScalar();

                    string pageSql = " OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY ";
                    cmd.Parameters.AddWithValue("@offset", (page - 1)*pageSize);
                    cmd.Parameters.AddWithValue("@pageSize",pageSize);
                    cmd.CommandText = string.Format(sql, fields) + whereSql + orderSql + pageSql;


                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Supplier supplier = new Supplier()
                            {
                                SupplierID = rdr.GetInt32(rdr.GetOrdinal("SupplierID")),
                                Name = rdr.GetString(rdr.GetOrdinal("Name")),
                                Description = rdr.GetString(rdr.GetOrdinal("Description")),
                                OwnerID = rdr.GetString(rdr.GetOrdinal("OwnerID")),
                                Phone = rdr.GetString(rdr.GetOrdinal("Phone")),
                                CategoryID = rdr.GetInt32(rdr.GetOrdinal("CategoryID")),
                                Capacity = rdr.GetInt32(rdr.GetOrdinal("Capacity")),
                                Address = rdr.GetString(rdr.GetOrdinal("Address")),
                                Cost = rdr.GetDecimal(rdr.GetOrdinal("Cost")),
                                SocialMedia = rdr.GetString(rdr.GetOrdinal("SocialMedia")),
                                VideoLink = rdr.GetString(rdr.GetOrdinal("VideoLink")),
                                BackgroundImageID = rdr.GetInt32(rdr.GetOrdinal("BackgroundImageID")),
                                City = rdr.GetString(rdr.GetOrdinal("City")),
                                Brand = rdr.GetString(rdr.GetOrdinal("Brand")),
                                Location = rdr.GetString(rdr.GetOrdinal("Location"))

                            };
                            suppliersList.Add(supplier);
                        }
                    }

                }
            }
            return suppliersList;


        }


        public void Create(Supplier supplier)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO [dbo].[Supplier]
           ([Name]
           ,[Description]
           ,[OwnerId]
           ,[Phone]
           ,[CategoryID]
           ,[Capacity]
           ,[Address]
           ,[Cost]
           ,[SocialMedia]
           ,[VideoLink]
           ,[BackgroundImageID]
           ,[City]
           ,[Brand]
           ,[Location]
           ,[Email])
     VALUES
           (@Name				
           ,@Description		
           ,@OwnerId			
           ,@Phone				
           ,@CategoryID			
           ,@Capacity			
           ,@Address			
           ,@Cost				
           ,@SocialMedia		
           ,@VideoLink			
           ,@BackgroundImageID	
           ,@City				
           ,@Brand				
           ,@Location
           ,@Email			)";



                    cmd.Parameters.AddWithValue("@Name", supplier.Name);
                    cmd.Parameters.AddWithValue("@Description", supplier.Description);
                    cmd.Parameters.AddWithValue("@OwnerId", supplier.OwnerID);
                    cmd.Parameters.AddWithValue("@Phone", supplier.Phone);
                    cmd.Parameters.AddWithValue("@CategoryID", supplier.CategoryID);
                    cmd.Parameters.AddWithValue("@Capacity", supplier.Capacity);
                    cmd.Parameters.AddWithValue("@Address", supplier.Address);
                    cmd.Parameters.AddWithValue("@Cost", supplier.Cost);
                    cmd.Parameters.AddWithValue("@SocialMedia", supplier.SocialMedia);
                    cmd.Parameters.AddWithValue("@VideoLink", supplier.VideoLink.Remove(0, 16));
                    cmd.Parameters.AddWithValue("@BackgroundImageID", supplier.BackgroundImageID);
                    cmd.Parameters.AddWithValue("@City", supplier.City);
                    cmd.Parameters.AddWithValue("@Brand", supplier.Brand);
                    cmd.Parameters.AddWithValue("@Location", supplier.Location);
                    cmd.Parameters.AddWithValue("@Email", supplier.Email);



                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Supplier supplier)
        {
            using (DbConnection conn = new SqlConnection(connectionString))
            {
                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE [dbo].[Supplier]
                                        SET [Name]					= @Name					
                                           ,[Description]			= @Description			
                                           
                                           ,[Phone]					= @Phone				
                                           ,[CategoryID]			= @CategoryID			
                                           ,[Capacity]				= @Capacity				
                                           ,[Address]				= @Address				
                                           ,[Cost]					= @Cost					
                                           ,[SocialMedia]			= @SocialMedia			
                                           ,[VideoLink]				= @VideoLink			
                                           ,[BackgroundImageID]		= @BackgroundImageID	
                                           ,[City]					= @City					
                                           ,[Brand]					= @Brand				
                                           ,[Location]				= @Location				
                                           ,[Email]				= @Email				
                                       WHERE [SupplierID]				= @SupplierID";

                    DbParameter sSupplierID = cmd.CreateParameter();
                    sSupplierID.ParameterName = "@SupplierID";
                    sSupplierID.Value = supplier.SupplierID;
                    sSupplierID.DbType = System.Data.DbType.Int32;
                    cmd.Parameters.Add(sSupplierID);


                    DbParameter sName = cmd.CreateParameter();
                    sName.ParameterName = "@Name";
                    sName.Value = supplier.Name;
                    sName.DbType = System.Data.DbType.String;
                    cmd.Parameters.Add(sName);

                    DbParameter sDescription = cmd.CreateParameter();
                    sDescription.ParameterName = "@Description";
                    sDescription.Value = supplier.Description;
                    sDescription.DbType = System.Data.DbType.String;
                    cmd.Parameters.Add(sDescription);
                    /*
                    DbParameter sOwnerId = cmd.CreateParameter();
                    sOwnerId.ParameterName = "@OwnerId";
                    sOwnerId.Value = supplier.OwnerID;
                    sOwnerId.DbType = System.Data.DbType.String;
                    cmd.Parameters.Add(sOwnerId);
                    */
                    DbParameter sPhone = cmd.CreateParameter();
                    sPhone.ParameterName = "@Phone";
                    sPhone.Value = supplier.Phone;
                    sPhone.DbType = System.Data.DbType.String;
                    cmd.Parameters.Add(sPhone);

                    DbParameter sCategoryID = cmd.CreateParameter();
                    sCategoryID.ParameterName = "@CategoryID";
                    sCategoryID.Value = supplier.CategoryID;
                    sCategoryID.DbType = System.Data.DbType.Int32;
                    cmd.Parameters.Add(sCategoryID);

                    DbParameter sCapacity = cmd.CreateParameter();
                    sCapacity.ParameterName = "@Capacity";
                    sCapacity.Value = supplier.Capacity;
                    sCapacity.DbType = System.Data.DbType.Int32;
                    cmd.Parameters.Add(sCapacity);

                    DbParameter sAddress = cmd.CreateParameter();
                    sAddress.ParameterName = "@Address";
                    sAddress.Value = supplier.Address;
                    sAddress.DbType = System.Data.DbType.String;
                    cmd.Parameters.Add(sAddress);

                    DbParameter sCost = cmd.CreateParameter();
                    sCost.ParameterName = "@Cost";
                    sCost.Value = supplier.Cost;
                    sCost.DbType = System.Data.DbType.Decimal;
                    cmd.Parameters.Add(sCost);

                    DbParameter sSocialMedia = cmd.CreateParameter();
                    sSocialMedia.ParameterName = "@SocialMedia";
                    sSocialMedia.Value = supplier.SocialMedia;
                    sSocialMedia.DbType = System.Data.DbType.String;
                    cmd.Parameters.Add(sSocialMedia);

                    DbParameter sVideoLink = cmd.CreateParameter();
                    sVideoLink.ParameterName = "@VideoLink";
                    if (supplier.VideoLink.Length > 17)
                    {
                        sVideoLink.Value = supplier.VideoLink.Remove(0, 16);
                    }
                    else
                    {
                        sVideoLink.Value = supplier.VideoLink;
                    }

                    sVideoLink.DbType = System.Data.DbType.String;
                    cmd.Parameters.Add(sVideoLink);

                    DbParameter sBackgroundImageID = cmd.CreateParameter();
                    sBackgroundImageID.ParameterName = "@BackgroundImageID";
                    sBackgroundImageID.Value = supplier.BackgroundImageID;
                    sBackgroundImageID.DbType = System.Data.DbType.Int32;
                    cmd.Parameters.Add(sBackgroundImageID);

                    DbParameter sCity = cmd.CreateParameter();
                    sCity.ParameterName = "@City";
                    sCity.Value = supplier.City;
                    sCity.DbType = System.Data.DbType.String;
                    cmd.Parameters.Add(sCity);

                    DbParameter sBrand = cmd.CreateParameter();
                    sBrand.ParameterName = "@Brand";
                    sBrand.Value = supplier.Brand;
                    sBrand.DbType = System.Data.DbType.String;
                    cmd.Parameters.Add(sBrand);

                    DbParameter sLocation = cmd.CreateParameter();
                    sLocation.ParameterName = "@Location";
                    sLocation.Value = supplier.Location;
                    sLocation.DbType = System.Data.DbType.String;
                    cmd.Parameters.Add(sLocation);

                    DbParameter sEmail = cmd.CreateParameter();
                    sEmail.ParameterName = "@Email";
                    sEmail.Value = supplier.Email;
                    sEmail.DbType = System.Data.DbType.String;
                    cmd.Parameters.Add(sEmail);



                    conn.Open();
                    cmd.ExecuteNonQuery();

                }
            }
        }



        public Supplier GetById(int id)
        {

            Supplier supplier = null;

            using (var conn = new SqlConnection(connectionString))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT   s.[SupplierID]
        ,s.[Name]
        ,s.[Description]
        ,s.[OwnerId]
        ,s.[Phone]
        ,s.[CategoryID]
        ,s.[Capacity]
        ,s.[Address]
        ,s.[Cost]
        ,s.[SocialMedia]
        ,s.[VideoLink]
        ,s.[BackgroundImageID]
        ,s.[City]
        ,s.[Brand]
        ,s.[Location]
        ,s.[Email]
FROM [dbo].[Supplier] s  WHERE s.[SupplierID]=@SupplierID";
                    cmd.Parameters.AddWithValue("@SupplierID", id);
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            /*
                            supplier = new Supplier()
                            {
                                SupplierID       =     id,
                                
                                Name             =reader.GetString(reader.GetOrdinal("Name")),
                                Description      =reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                                OwnerID          =reader.GetInt32(reader.GetOrdinal("OwnerId")),
                                Phone            =reader.GetString(reader.GetOrdinal("Phone")),
                                CategoryID       =reader.GetInt32(reader.GetOrdinal("CategoryID")),
                                Capacity         =reader.GetInt32(reader.GetOrdinal("Capacity")),
                                Address          =reader.GetString(reader.GetOrdinal("Address")),
                                Cost             =reader.GetDecimal(reader.GetOrdinal("Cost")),
                                SocialMedia      =reader.GetString(reader.GetOrdinal("SocialMedia")),
                                VideoLink        =reader.GetString(reader.GetOrdinal("VideoLink")),
                                BackgroundImageID=reader.GetInt32(reader.GetOrdinal("BackgroundImageID")),
                                City             =reader.GetString(reader.GetOrdinal("City")),
                                Brand            =reader.GetString(reader.GetOrdinal("Brand")),
                                Location         =reader.GetString(reader.GetOrdinal("Location"))

                            };
                            */

                            supplier = new Supplier()
                            {
                                SupplierID = id,
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                                OwnerID = reader.GetString(reader.GetOrdinal("OwnerID")),
                                Phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader.GetString(reader.GetOrdinal("Phone")),
                                CategoryID = reader.GetInt32(reader.GetOrdinal("CategoryID")),
                                Capacity = reader.GetInt32(reader.GetOrdinal("Capacity")),
                                Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString(reader.GetOrdinal("Address")),
                                Cost = reader.IsDBNull(reader.GetOrdinal("Cost")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Cost")),
                                SocialMedia = reader.IsDBNull(reader.GetOrdinal("SocialMedia")) ? null : reader.GetString(reader.GetOrdinal("SocialMedia")),
                                VideoLink = reader.IsDBNull(reader.GetOrdinal("VideoLink")) ? null : reader.GetString(reader.GetOrdinal("VideoLink")),
                                BackgroundImageID = reader.IsDBNull(reader.GetOrdinal("BackgroundImageID")) ? 0 : reader.GetInt32(reader.GetOrdinal("BackgroundImageID")),
                                City = reader.IsDBNull(reader.GetOrdinal("City")) ? null : reader.GetString(reader.GetOrdinal("City")),
                                Brand = reader.IsDBNull(reader.GetOrdinal("Brand")) ? null : reader.GetString(reader.GetOrdinal("Brand")),
                                Location = reader.IsDBNull(reader.GetOrdinal("Location")) ? null : reader.GetString(reader.GetOrdinal("Location")),
                                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email"))

                            };


                        }

                    }
                }
            }
            return supplier;
        }

        public void Delete(int id)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                using (var cmd = conn.CreateCommand())
                {
                  
                    cmd.CommandText = "DELETE FROM [dbo].[Image] WHERE SupplierID=@SupplierID;DELETE FROM [dbo].[Supplier] WHERE SupplierID=@SupplierID;";
                    cmd.Parameters.AddWithValue("@SupplierID", id);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

        }
        public IList<Supplier> GetByOwnerID(string ownerID)
        {
            IList<Supplier> suppliersList = new List<Supplier>();
            using (var connection = new SqlConnection(connectionString))
            {

                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = @"SELECT  s.[SupplierID] 
                                                ,s.[Name]
                                                ,s.[Description]
                                                ,s.[OwnerId]
                                                ,s.[Phone]
                                                ,s.[CategoryID]
                                                ,s.[Capacity]
                                                ,s.[Address]
                                                ,s.[Cost]
                                                ,s.[SocialMedia]
                                                ,s.[VideoLink]
                                                ,s.[BackgroundImageID]
                                                ,s.[City]
                                                ,s.[Brand]
                                                ,s.[Location]
                                        FROM [dbo].[Supplier] s left join dbo.Category c on c.CategoryID=s.CategoryID WHERE s.OwnerId=@OwnerId";
                    cmd.Parameters.AddWithValue("@OwnerId", ownerID);

                    connection.Open();


                    using (DbDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Supplier supplier = new Supplier()
                            {
                                SupplierID = rdr.GetInt32(rdr.GetOrdinal("SupplierID")),
                                Name = rdr.GetString(rdr.GetOrdinal("Name")),
                                Description = rdr.GetString(rdr.GetOrdinal("Description")),
                                OwnerID = rdr.GetString(rdr.GetOrdinal("OwnerID")),
                                Phone = rdr.GetString(rdr.GetOrdinal("Phone")),
                                CategoryID = rdr.GetInt32(rdr.GetOrdinal("CategoryID")),
                                Capacity = rdr.GetInt32(rdr.GetOrdinal("Capacity")),
                                Address = rdr.GetString(rdr.GetOrdinal("Address")),
                                Cost = rdr.GetDecimal(rdr.GetOrdinal("Cost")),
                                SocialMedia = rdr.GetString(rdr.GetOrdinal("SocialMedia")),
                                VideoLink = rdr.GetString(rdr.GetOrdinal("VideoLink")),
                                //BackgroundImageID   =rdr.IsDBNull(rdr.GetOrdinal("BackgroundImageID"))? 0 :(rdr.GetOrdinal("BackgroundImageID")),
                                City = rdr.GetString(rdr.GetOrdinal("City")),
                                Brand = rdr.GetString(rdr.GetOrdinal("Brand")),
                                Location = rdr.GetString(rdr.GetOrdinal("Location"))
                            };

                            suppliersList.Add(supplier);
                        }
                    }
                }
            }
            return suppliersList;
        }
      
    }
}