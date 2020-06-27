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
    public class ProductAttributeDAL : IProductAttributesDAL
    {
        private string connectionString;
        public ProductAttributeDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public int Add(ProductAttributes data)
        {
            int attributeId = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO ProductAttributes
                                           (
                                                AttributeName,
                                                AttributeValues,
                                                DisplayOrder
                                           )
                                           VALUES
                                           (
                                                @AttributeName,
                                                @AttributeValues,
                                                @DisplayOrder
                                           );
                                           SELECT @@IDENTITY;";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@AttributeName", data.AttributeName);
                cmd.Parameters.AddWithValue("@AttributeValues", data.AttributeValues);
                cmd.Parameters.AddWithValue("@DisplayOrder", data.DisplayOrder);
                attributeId = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();
            }
            return attributeId;
        }

        public bool Delete(int[] attributeIDs)
        {
            bool result = true;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM ProductAttributes
                                            WHERE(AttributeID = @AttributeID)
                                           ";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.Add("@AttributeID", SqlDbType.Int);
                foreach (int attributeId in attributeIDs)
                {
                    cmd.Parameters["@AttributeID"].Value = attributeId;
                    cmd.ExecuteNonQuery();
                }

                connection.Close();
            }
            return result;
        }

        public List<ProductAttributes> Get(int productID)
        {
            List<ProductAttributes> data = new List<ProductAttributes>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT *
                                    FROM            ProductAttributes 
						                 where ProductID = @ProductID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@ProductID", productID);
                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dbReader.Read())
                    {
                        ProductAttributes productAttributes = new ProductAttributes()
                        {
                            AttributeID = Convert.ToInt32(dbReader["AttributeID"]),
                            ProductID = Convert.ToInt32(dbReader["ProductID"]),                       
                            AttributeName = Convert.ToString(dbReader["AttributeName"]),
                            AttributeValues = Convert.ToString(dbReader["AttributeValues"]),
                            DisplayOrder = Convert.ToInt32(dbReader["DisplayOrder"]),
                        };
                        data.Add(productAttributes);
                    }
                }
                connection.Close();
            }
            return data;
        }
        public ProductAttributes GetAttribute(int AttributeID)
        {
            ProductAttributes data = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT *
                                    FROM            ProductAttributes 
						                 where AttributeID = @AttributeID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@AttributeID", AttributeID);
                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dbReader.Read())
                    {
                        data = new ProductAttributes()
                        {
                            AttributeID = Convert.ToInt32(dbReader["AttributeID"]),
                            ProductID = Convert.ToInt32(dbReader["ProductID"]),
                            AttributeName = Convert.ToString(dbReader["AttributeName"]),
                            AttributeValues = Convert.ToString(dbReader["AttributeValues"]),
                            DisplayOrder = Convert.ToInt32(dbReader["DisplayOrder"]),
                        };
                       
                    }
                }
                connection.Close();
            }
            return data;
        }

        public bool Update(ProductAttributes data)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE ProductAttributes
                                    SET
                                    ProductID = @ProductID,
                                    AttributeName = @AttributeName,
                                    AttributeValues = @AttributeValues,
                                    DisplayOrder = @DisplayOrder
                                    WHERE
                                    AttributeID = @AttributeID;";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;

                cmd.Parameters.AddWithValue("@ProductID", data.ProductID);
                cmd.Parameters.AddWithValue("@AttributeName", data.AttributeName);
                cmd.Parameters.AddWithValue("@AttributeValues", data.AttributeValues);
                cmd.Parameters.AddWithValue("@DisplayOrder", data.DisplayOrder);
                cmd.Parameters.AddWithValue("@AttributeID", data.AttributeID);
                

                rowsAffected = Convert.ToInt32(cmd.ExecuteNonQuery());

                connection.Close();
            }
            return rowsAffected > 0;
        }
    }
}
