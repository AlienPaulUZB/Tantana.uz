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
    public class FeedbackRepository
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
        public IList<Feedback> GetSupplierFeedbacks(int supId)
        {
            IList<Feedback> feedbacks = new List<Feedback>();
            using(var connection=new SqlConnection(connectionString))
            {
                using(var command = connection.CreateCommand())
                {
                    command.CommandText = @"SELECT [FeedbackID]
                                                ,[SupplierID]
                                                ,[Subject]
                                                ,[Message]
                                                ,[CreatedDate]
                                                ,[UserID]
                                                ,[Rating]
                                            FROM [dbo].[Feedback] WHERE [SupplierID]=@supplierID";
                    command.Parameters.AddWithValue("@supplierID",supId);

                    connection.Open();

                    using (DbDataReader rdr = command.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Feedback feedback = new Feedback()
                            {
                                FeedbackId=rdr.GetInt32(rdr.GetOrdinal("FeedbackID")),
                                SupplierId=rdr.GetInt32(rdr.GetOrdinal("SupplierID")),
                                Subject=rdr.GetString(rdr.GetOrdinal("Subject")),
                                Message=rdr.GetString(rdr.GetOrdinal("Message")),
                                CreatedTime=rdr.GetDateTime(rdr.GetOrdinal("CreatedDate")),
                                UserID=rdr.GetString(rdr.GetOrdinal("UserID")),
                                Rating=rdr.GetInt32(rdr.GetOrdinal("Rating"))
                            };

                            feedbacks.Add(feedback);
                        }
                    }
                }
            }
            return feedbacks;
        } 

        public void Create(Feedback feedback)
        {
            using(var conn=new SqlConnection(connectionString))
            {
                using(var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO [dbo].[Feedback]
           ([SupplierID]
           ,[Subject]
           ,[Message]
           ,[CreatedDate]
           ,[UserID]
           ,[Rating])
     VALUES
           (@SupplierID
           ,@Subject
           ,@Message
           ,@CreatedDate
           ,@UserID
           ,@Rating)";

                    cmd.Parameters.AddWithValue("@SupplierID", feedback.SupplierId);
                    cmd.Parameters.AddWithValue("@Subject", feedback.Subject);
                    cmd.Parameters.AddWithValue("@Message", feedback.Message);
                    cmd.Parameters.AddWithValue("@CreatedDate", feedback.CreatedTime);
                    cmd.Parameters.AddWithValue("@UserID", feedback.UserID);
                    cmd.Parameters.AddWithValue("@Rating", feedback.Rating);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                }
            }
        }
        public void Delete(int id)
        {
            using(var conn=new SqlConnection(connectionString))
            {
                using(var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM [dbo].[Feedback] WHERE FeedbackID=@FeedbackID";
                    cmd.Parameters.AddWithValue("@FeedbackID", id);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public decimal AverageRate(int supId)
        {
            var average = 0;
            using(var connection=new SqlConnection(connectionString))
            {
                using(var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT AVG(Rating) FROM dbo.Feedback WHERE SupplierId=@SupplierId";
                    cmd.Parameters.AddWithValue("@SupplierId",supId);

                    connection.Open();
                    cmd.ExecuteNonQuery();
                    using(DbDataReader rdr=cmd.ExecuteReader()){
                        while (rdr.Read())
                        {
                            average = rdr.GetInt32(0);
                                
                            
                        }
                        
                        
                    }
                }
            }
            return average;
        }

      
    }
}