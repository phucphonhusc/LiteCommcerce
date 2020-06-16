using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers.SqlServer
{
    public class EmployeeDAL : IEmployeeDAL
    {
        private string connectionString;
        
        public EmployeeDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public int Add(Employee data)
        {
            int employeeId = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Employees
                                          (
	                                          FirstName,
	                                          LastName,
	                                          Title,
	                                          BirthDate,
	                                          HireDate,
	                                          Email,
	                                          Address,
	                                          City,
                                              Country,
                                              HomePhone,
                                              Notes,
                                              PhotoPath,
                                              Password
                                          )
                                          VALUES
                                          (
	                                          @FirstName,
	                                          @LastName,
	                                          @Title,
	                                          @BirthDate,
	                                          @HireDate,
	                                          @Email,
	                                          @Address,
	                                          @City,
                                              @Country,
                                              @HomePhone,
                                              @Notes,
                                              @PhotoPath,
                                              @Password
                                          );
                                          SELECT @@IDENTITY;";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@FirstName", data.FirstName);
                cmd.Parameters.AddWithValue("@LastName", data.LastName);
                cmd.Parameters.AddWithValue("@Title", data.Title);
                cmd.Parameters.AddWithValue("@BirthDate", data.BirthDate);
                cmd.Parameters.AddWithValue("@HireDate", data.HireDate);
                cmd.Parameters.AddWithValue("@Email", data.Email);
                cmd.Parameters.AddWithValue("@Address", data.Address);
                cmd.Parameters.AddWithValue("@City", data.City);
                cmd.Parameters.AddWithValue("@Country", data.Country);
                cmd.Parameters.AddWithValue("@HomePhone", data.HomePhone);
                cmd.Parameters.AddWithValue("@Notes", data.Notes);
                cmd.Parameters.AddWithValue("@PhotoPath", data.PhotoPath);
                cmd.Parameters.AddWithValue("@Password", data.Password);
                employeeId = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();
            }
            return employeeId;
        }

        public int Count(string searchValue, string country)
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
                    cmd.CommandText = @"select count (*)  from Employees 
                            where ((@searchValue = N'') or (FirstName like @searchValue) or(LastName like @searchValue) )and ((@country = N'') or (Country = @country))
												  
                    ";
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@searchValue", searchValue);
                    cmd.Parameters.AddWithValue("@country", country);
                    rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                }
                connection.Close();
            }
            return rowCount;
        }

        public bool Delete(int[] employeeIDs)
        {
            bool result = true;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM Employees WHERE(EmployeeID = @employeeId )
                                    AND (EmployeeID NOT IN(SELECT EmployeeID FROM Orders))";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.Add("@employeeId", SqlDbType.Int);
                foreach (int employeeId in employeeIDs)
                {
                    cmd.Parameters["@employeeId"].Value = employeeId;
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
            return result;
        }

        public Employee Get(int employeeID)
        {
            Employee data = null;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM Employees WHERE EmployeeID = @employeeID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@employeeID", employeeID);
                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dbReader.Read())
                    {
                        data = new Employee()
                        {
                            EmployeeID = Convert.ToInt32(dbReader["EmployeeID"]),
                            FirstName = Convert.ToString(dbReader["FirstName"]),
                            LastName = Convert.ToString(dbReader["LastName"]),
                            Title = Convert.ToString(dbReader["Title"]),
                            BirthDate = Convert.ToDateTime(dbReader["BirthDate"]),
                            HireDate = Convert.ToDateTime(dbReader["HireDate"]),
                            Email = Convert.ToString(dbReader["Email"]),
                            Address = Convert.ToString(dbReader["Address"]),
                            City = Convert.ToString(dbReader["City"]),
                            Country = Convert.ToString(dbReader["Country"]),
                            Notes = Convert.ToString(dbReader["Notes"]),
                            HomePhone = Convert.ToString(dbReader["HomePhone"]),
                            PhotoPath = Convert.ToString(dbReader["PhotoPath"]),
                            Password = Convert.ToString(dbReader["Password"])
                        };
                    }
                }
                connection.Close();
            }
            return data;
        }

        public List<Employee> List(int page, int pageSize, string searchValue, string country)
        {
            List<Employee> data = new List<Employee>();
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
		                        ROW_NUMBER() over ( order by EmployeeID) AS RowNumber
	                        from Employees
	                        where ((@searchValue = N'') or (FirstName like @searchValue) or(LastName like @searchValue) )and ((@country = N'') or (Country = @country))

                        ) as t
                        where t.RowNumber between (@page -1) *@pageSize +1 and @page * @pageSize
                        order by t.RowNumber";
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@page", page);
                    cmd.Parameters.AddWithValue("@pageSize", pageSize);
                    cmd.Parameters.AddWithValue("@searchValue", searchValue);
                    cmd.Parameters.AddWithValue("@country", country);

                    using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dbReader.Read())
                        {
                            data.Add(new Employee()
                            {
                                EmployeeID = Convert.ToInt32(dbReader["EmployeeID"]),
                                FirstName = Convert.ToString(dbReader["FirstName"]),
                                LastName = Convert.ToString(dbReader["LastName"]),
                                Title = Convert.ToString(dbReader["Title"]),
                                BirthDate = Convert.ToDateTime(dbReader["BirthDate"]),
                                HireDate = Convert.ToDateTime(dbReader["HireDate"]),
                                Email = Convert.ToString(dbReader["Email"]),
                                Address = Convert.ToString(dbReader["Address"]),
                                City = Convert.ToString(dbReader["City"]),
                                Country = Convert.ToString(dbReader["Country"]),
                                Notes = Convert.ToString(dbReader["Notes"]),
                                HomePhone = Convert.ToString(dbReader["HomePhone"]),
                                PhotoPath = Convert.ToString(dbReader["PhotoPath"]),
                                Password = Convert.ToString(dbReader["Password"])
                            });
                        }
                    }

                }
                connection.Close();

            }
            return data;
        }

        public bool Update(Employee data)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Employees
                                    SET FirstName = @FirstName, 
                                        LastName = @LastName,
                                        Title = @Title,
                                        BirthDate = @BirthDate,
                                        HireDate = @HireDate,
                                        Email = @Email,
                                        Address = @Address,
                                        City = @City,
                                        Country = @Country,
                                        HomePhone = @HomePhone,
                                        Notes = @Notes,
                                        PhotoPath = @PhotoPath,
                                        Password = @Password
                                    WHERE EmployeeID = @EmployeeID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@FirstName", data.FirstName);
                cmd.Parameters.AddWithValue("@LastName", data.LastName);
                cmd.Parameters.AddWithValue("@Title", data.Title);
                cmd.Parameters.AddWithValue("@BirthDate", data.BirthDate);
                cmd.Parameters.AddWithValue("@HireDate", data.HireDate);
                cmd.Parameters.AddWithValue("@Email", data.Email);
                cmd.Parameters.AddWithValue("@Address", data.Address);
                cmd.Parameters.AddWithValue("@City", data.City);
                cmd.Parameters.AddWithValue("@Country", data.Country);
                cmd.Parameters.AddWithValue("@HomePhone", data.HomePhone);
                cmd.Parameters.AddWithValue("@Notes", data.Notes);
                cmd.Parameters.AddWithValue("@PhotoPath", data.PhotoPath);
                cmd.Parameters.AddWithValue("@Password", data.Password);
                cmd.Parameters.AddWithValue("@EmployeeID", data.EmployeeID);

                rowsAffected = Convert.ToInt32(cmd.ExecuteNonQuery());

                connection.Close();
            }

            return rowsAffected > 0;
        }

    }
}
