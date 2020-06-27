﻿using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers.SqlServer
{
    public class CountryDAL : ICountryDAL
    {
        private string connectionString;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public CountryDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public List<Country> List()
        {
            List<Country> data = new List<Country>();
            
            //TODO: Truy vấn dữ liệu từ CSDL...
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = @"select * from Countries ORDER BY CountryName ASC
                        ";
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = connection;
                    

                    using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dbReader.Read())
                        {
                            data.Add(new Country()
                            {
                                CountryName = Convert.ToString(dbReader["CountryName"])
                          
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
