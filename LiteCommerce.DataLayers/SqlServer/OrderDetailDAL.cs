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
    public class OrderDetailDAL : IOrderDetailDAL
    {
        private string connectionString;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public OrderDetailDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public List<OrderDetail> GetDetail(int orderID)
        {
            List<OrderDetail> data = new List<OrderDetail>();

            //TODO: Truy vấn dữ liệu từ CSDL...
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = @"select * from OrderDetails where OrderID = @OrderID
                        ";
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@OrderID", orderID);

                    using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dbReader.Read())
                        {
                            data.Add(new OrderDetail()
                            {
                                OrderID = Convert.ToInt32(dbReader["OrderID"]),
                                ProductID = Convert.ToInt32(dbReader["ProductID"]),
                                UnitPrice = Convert.ToInt32(dbReader["UnitPrice"]),
                                Quantity = Convert.ToInt32(dbReader["Quantity"]),
                                Discount = Convert.ToSingle(dbReader["Discount"]),
                               
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
