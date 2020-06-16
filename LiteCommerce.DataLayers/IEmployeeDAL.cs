using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
    public interface IEmployeeDAL
    {
        int Add(Employee data);
        bool Update(Employee data);
        bool Delete(int[] employeeIDs);
        Employee Get(int employeeID);
        List<Employee> List(int page, int pageSize, string searchValue, string country);
        int Count(string searchValue, string country);
    }
}
