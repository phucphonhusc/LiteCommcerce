using LiteCommerce.DataLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.BusinessLayers
{
    public static class HumanResourceBLL
    {
        private static IEmployeeDAL EmployeeDB { get; set; }
        public static void Initialize(string connectionString)
        {
            EmployeeDB = new DataLayers.SqlServer.EmployeeDAL(connectionString);
        }
        public static List<Employee> Employee_List(int page, int pageSize, string searchValue, string country)
        {
            return EmployeeDB.List(page, pageSize, searchValue, country);
        }
        public static Employee Employee_Get(int employeeID)
        {
            return EmployeeDB.Get(employeeID);
        }
        public static int Employee_Add(Employee data)
        {
            return EmployeeDB.Add(data);
        }
        public static bool Employee_Delete(int[] employeeIDs)
        {
            return EmployeeDB.Delete(employeeIDs);
        }
        public static bool Employee_Update(Employee data)
        {
            return EmployeeDB.Update(data);
        }
        public static int Employee_Count(string searchValue, string country)
        {
            return EmployeeDB.Count(searchValue, country);
        }
    }
}
