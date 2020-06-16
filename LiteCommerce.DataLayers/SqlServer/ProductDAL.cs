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
    public class ProductDAL : IProductDAL
    {
        private string connectionString;
        public ProductDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public int Add(Product data)
        {
            int productId = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Products
                                           (
                                                ProductName,
                                                SupplierID,
                                                CategoryID,
                                                QuantityPerUnit,
                                                UnitPrice,
                                                Descriptions,
                                                PhotoPath
                                           )
                                           VALUES
                                           (
                                                @ProductName,
                                                @SupplierID,
                                                @CategoryID,
                                                @QuantityPerUnit,
                                                @UnitPrice,
                                                @Descriptions,
                                                @PhotoPath
                                           );
                                           SELECT @@IDENTITY;";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@ProductName", data.ProductName);
                cmd.Parameters.AddWithValue("@SupplierID", data.SupplierID);
                cmd.Parameters.AddWithValue("@CategoryID", data.CategoryID);
                cmd.Parameters.AddWithValue("@QuantityPerUnit", data.QuantityPerUnit);
                cmd.Parameters.AddWithValue("@UnitPrice", data.UnitPrice);
                cmd.Parameters.AddWithValue("@Descriptions", data.Descriptions);
                cmd.Parameters.AddWithValue("@PhotoPath", data.PhotoPath);

                productId = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();
            }
            return productId;
        }

        public int Count(string searchValue, int category, int supplier)
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
                    cmd.CommandText = @"select COUNT(*) from Products where ((@searchValue = N'') or (ProductName like @searchValue) )and ((@category = N'') or (CategoryID = @category)) and
												  ((@supplier = N'') or (SupplierID = @supplier))
                    ";
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@searchValue", searchValue);
                    cmd.Parameters.AddWithValue("@category", category);
                    cmd.Parameters.AddWithValue("@supplier", supplier);
                    rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                }
                connection.Close();
            }
            return rowCount;
        }
   

        public bool Delete(int[] productIDs)
        {
            bool result = true;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM Products
                                            WHERE(ProductID = @ProductID)
                                            AND(ProductID NOT IN(SELECT ProductID FROM ProductAttributes))";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.Add("@ProductID", SqlDbType.Int);
                foreach (int productId in productIDs)
                {
                    cmd.Parameters["@ProductID"].Value = productId;
                    cmd.ExecuteNonQuery();
                }

                connection.Close();
            }
            return result;
        }
    

        public Product Get(int productID)
        {
            Product data = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT        *
                                    FROM            
                                         Products 
						                 where ProductID = @ProductID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@ProductID", productID);
                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dbReader.Read())
                    {
                        data = new Product()
                        {
                            ProductID = Convert.ToInt32(dbReader["ProductID"]),
                            ProductName = Convert.ToString(dbReader["ProductName"]),
                            SupplierID = Convert.ToInt32(dbReader["SupplierID"]),
                            CategoryID = Convert.ToInt32(dbReader["CategoryID"]),
                            QuantityPerUnit = Convert.ToString(dbReader["QuantityPerUnit"]),
                            UnitPrice = Convert.ToInt32(dbReader["UnitPrice"]),
                            Descriptions = Convert.ToString(dbReader["Descriptions"]),
                            PhotoPath = Convert.ToString(dbReader["PhotoPath"]),
                            
                        };
                    }
                }
                connection.Close();
            }
            return data;
        }

        public List<Product> List(int page, int pageSize, string searchValue, int category, int supplier)
        {
            List<Product> data = new List<Product>();
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = @"select *
                                        from
                                        (
	                                        select *,
	                                        ROW_NUMBER() over(order by ProductID) as RowNumber
	                                        from Products
	                                        where ((@searchValue = N'') or(ProductName like @searchValue)) and
												  ((@category = N'') or (CategoryID = @category)) and
												  ((@supplier = N'') or (SupplierID = @supplier))
                                        ) as t
                                        where t.RowNumber between (@page-1)*@pageSize + 1 and @page*@pageSize
                                        order by t.RowNumber";
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@page", page);
                    cmd.Parameters.AddWithValue("@pageSize", pageSize);
                    cmd.Parameters.AddWithValue("@searchValue", searchValue);
                    cmd.Parameters.AddWithValue("@category", category);
                    cmd.Parameters.AddWithValue("@supplier", supplier);


                    using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dbReader.Read())
                        {
                            data.Add(new Product()
                            {
                                ProductID = Convert.ToInt32(dbReader["ProductID"]),
                                ProductName = Convert.ToString(dbReader["ProductName"]),
                                SupplierID = Convert.ToInt32(dbReader["SupplierID"]),
                                CategoryID = Convert.ToInt32(dbReader["CategoryID"]),
                                QuantityPerUnit = Convert.ToString(dbReader["QuantityPerUnit"]),
                                UnitPrice = Convert.ToInt32(dbReader["UnitPrice"]),
                                Descriptions = Convert.ToString(dbReader["Descriptions"]),
                                PhotoPath = Convert.ToString(dbReader["PhotoPath"]),
                            });
                        }
                    }
                }
                connection.Close();
            }
            return data;
        }

        public bool Update(Product data)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Products
                                    SET
                                    ProductName = @ProductName,
                                    SupplierID = @SupplierID,
                                    CategoryID = @CategoryID,
                                    QuantityPerUnit = @QuantityPerUnit,
                                    UnitPrice = @UnitPrice,
                                    Descriptions = @Descriptions,
                                    PhotoPath = @PhotoPath
                                    WHERE
                                    ProductID = @ProductID;";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;

                cmd.Parameters.AddWithValue("@ProductName", data.ProductName);
                cmd.Parameters.AddWithValue("@SupplierID", data.SupplierID);
                cmd.Parameters.AddWithValue("@CategoryID", data.CategoryID);
                cmd.Parameters.AddWithValue("@QuantityPerUnit", data.QuantityPerUnit);
                cmd.Parameters.AddWithValue("@UnitPrice", data.UnitPrice);
                cmd.Parameters.AddWithValue("@Descriptions", data.Descriptions);
                cmd.Parameters.AddWithValue("@PhotoPath", data.PhotoPath);
                cmd.Parameters.AddWithValue("@ProductID", data.ProductID);

                rowsAffected = Convert.ToInt32(cmd.ExecuteNonQuery());

                connection.Close();
            }
            return rowsAffected > 0;
        }
    }
}
