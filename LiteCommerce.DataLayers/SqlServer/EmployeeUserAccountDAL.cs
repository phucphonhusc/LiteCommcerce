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
    /// <summary>
    /// Kiem tra thong tin dang nhap cua nhan vien
    /// </summary>
    public class EmployeeUserAccountDAL : IUserAccountDAL
    {
        private string connectionString;
        public EmployeeUserAccountDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        /// <summary>
        /// Authorize nhan vien
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public UserAccount Authorize(string userName, string password)
        {
            UserAccount data = null;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM Employees WHERE Email = @email AND Password = @password";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@email", userName);
                cmd.Parameters.AddWithValue("@password", password);
                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dbReader.Read())
                    {
                        data = new UserAccount()
                        {
                            UserID = Convert.ToString(dbReader["Email"]),
                            FullName = Convert.ToString(dbReader["FirstName"])+" "+Convert.ToString(dbReader["LastName"]),
                            
                            Photo = Convert.ToString(dbReader["PhotoPath"]),
                            GroupName = Convert.ToString(dbReader["GroupName"])



                        };
                    }
                }
                connection.Close();
            }
            return data;
            /*return new UserAccount()
            {
                UserID = userName,
                FullName = "Phuc Phon",
                Photo = "ipx.jpg"
            };*/
        }
    }
}
