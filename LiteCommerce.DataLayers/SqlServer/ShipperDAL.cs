using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers.SqlServer
{
    public class ShipperDAL : IShipperDAL
    {
        private string connectionString;
        public ShipperDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public int Add(Shipper data)
        {
            int shipperId = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Shippers
                                          (
	                                          CompanyName,
	                                          Phone                      
                                          )
                                          VALUES
                                          (
	                                          @CompanyName,
	                                          @Phone
                                          );
                                          SELECT @@IDENTITY;";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@CompanyName", data.CompanyName);
                cmd.Parameters.AddWithValue("@Phone", data.Phone);
                
                shipperId = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();
            }
            return shipperId;
        }

        public int Count(string searchValue)
        {
            int rowCount = 0;
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = "%" + searchValue + "%";
            }
            //TODO: Truy vấn dữ liệu từ CSDL...
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = @"select count (*) from Shippers 
                    where (@searchValue = N'') or (CompanyName like @searchValue)
                    ";
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@searchValue", searchValue);
                    rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                }
                connection.Close();
            }
            return rowCount;
        }

        public bool Delete(int[] shipperIDs)
        {
            bool result = true;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM Shippers WHERE(ShipperID = @shipperId )
                                    AND (ShipperID NOT IN(SELECT ShipperID FROM Orders))";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.Add("@shipperId", SqlDbType.Int);
                foreach (int shipperId in shipperIDs)
                {
                    cmd.Parameters["@shipperId"].Value = shipperId;
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
            return result;
        }

        public Shipper Get(int shipperID)
        {
            Shipper data = null;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM Shippers WHERE ShipperID = @shipperID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@shipperID", shipperID);
                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dbReader.Read())
                    {
                        data = new Shipper()
                        {
                            ShipperID = Convert.ToInt32(dbReader["ShipperID"]),
                            CompanyName = Convert.ToString(dbReader["CompanyName"]),
                            Phone = Convert.ToString(dbReader["Phone"])
                        };
                    }
                }
                connection.Close();
            }
            return data;
        }

        public List<Shipper> List(string searchValue)
        {
            List<Shipper> data = new List<Shipper>();
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = "%" + searchValue + "%";
            }
            //TODO: Truy vấn dữ liệu từ CSDL...
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = @"select *
                        from
                        (
	                        select * ,
		                        ROW_NUMBER() over ( order by ShipperID) AS RowNumber
	                        from Shippers
	                        where (@searchValue = N'') or (CompanyName like @searchValue)

                        ) as t
                        order by t.RowNumber";
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@searchValue", searchValue);

                    using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dbReader.Read())
                        {
                            data.Add(new Shipper()
                            {
                                ShipperID = Convert.ToInt32(dbReader["ShipperID"]),
                                CompanyName = Convert.ToString(dbReader["CompanyName"]),
                                Phone = Convert.ToString(dbReader["Phone"])
                                
                            });
                        }
                    }

                }
                connection.Close();

            }
            return data;
        }

        public bool Update(Shipper data)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Shippers
                                    SET CompanyName = @CompanyName, 
                                        Phone = @Phone                                        
                                    WHERE ShipperID = @ShipperID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@CompanyName", data.CompanyName);
                cmd.Parameters.AddWithValue("@Phone", data.Phone);
                cmd.Parameters.AddWithValue("@ShipperID", data.ShipperID);

                rowsAffected = Convert.ToInt32(cmd.ExecuteNonQuery());

                connection.Close();
            }

            return rowsAffected > 0;
        }
    }
}
