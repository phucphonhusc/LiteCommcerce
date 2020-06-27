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
    public class OrderDAL : IOrderDAL
    {
        private string connectionString;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public OrderDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public int Count(string searchValue, string customer)
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
                    cmd.CommandText = @"select count (*) from Orders 
                    where ((@searchValue = N'') or (OrderID like @searchValue))  and ((@customer = N'') or (CustomerID = @customer))
                                                        
                    ";
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@searchValue", searchValue);
                    cmd.Parameters.AddWithValue("@customer", customer);
                  
                    rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                }
                connection.Close();
            }
            return rowCount;
        }

        public Order Get(int orderID)
        {
            Order data = null;
            
            //TODO: Truy vấn dữ liệu từ CSDL...
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = @"select * from Orders where OrderID = @OrderID
                        ";
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@OrderID", orderID);

                    using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dbReader.Read())
                        {
                            data = new Order()
                            {
                                OrderID = Convert.ToInt32(dbReader["OrderID"]),
                                CustomerID = Convert.ToString(dbReader["CustomerID"]),
                                EmployeeID = Convert.ToInt32(dbReader["EmployeeID"]),
                                OrderDate = Convert.ToDateTime(dbReader["OrderDate"]),
                                RequiredDate = Convert.ToDateTime(dbReader["RequiredDate"]),
                                ShippedDate = Convert.ToDateTime(dbReader["ShippedDate"]),
                                ShipperID = Convert.ToInt32(dbReader["ShipperID"]),
                                Freight = Convert.ToInt32(dbReader["Freight"]),
                                ShipAddress = Convert.ToString(dbReader["ShipAddress"]),
                                ShipCity = Convert.ToString(dbReader["ShipCity"]),
                                ShipCountry = Convert.ToString(dbReader["ShipCountry"]),
                            };
                        }
                    }

                }
                connection.Close();

            }
            return data;
        }

        public List<Order> List(int page, int pageSize, string searchValue, string customer)
        {
            List<Order> data = new List<Order>();
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
		                        ROW_NUMBER() over ( order by OrderID) AS RowNumber
	                        from Orders
	                        where ((@searchValue = N'') or (OrderID like @searchValue)) and ((@customer = N'') or (CustomerID = @customer))
                                                        

                        ) as t
                        where t.RowNumber between (@page -1) *@pageSize +1 and @page * @pageSize
                        order by t.RowNumber";
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@page", page);
                    cmd.Parameters.AddWithValue("@pageSize", pageSize);
                    cmd.Parameters.AddWithValue("@searchValue", searchValue);
                    cmd.Parameters.AddWithValue("@customer", customer);
                    

                    using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dbReader.Read())
                        {
                            data.Add(new Order()
                            {
                                OrderID = Convert.ToInt32(dbReader["OrderID"]),
                                CustomerID = Convert.ToString(dbReader["CustomerID"]),
                                EmployeeID = Convert.ToInt32(dbReader["EmployeeID"]),
                                OrderDate = Convert.ToDateTime(dbReader["OrderDate"]),
                                RequiredDate = Convert.ToDateTime(dbReader["RequiredDate"]),
                                ShippedDate = Convert.ToDateTime(dbReader["ShippedDate"]),
                                ShipperID = Convert.ToInt32(dbReader["ShipperID"]),
                                Freight = Convert.ToInt32(dbReader["Freight"]),
                                ShipAddress = Convert.ToString(dbReader["ShipAddress"]),
                                ShipCity = Convert.ToString(dbReader["ShipCity"]),
                                ShipCountry = Convert.ToString(dbReader["ShipCountry"]),
                            });
                        }
                    }

                }
                connection.Close();

            }
            return data;
        }
    }
}
