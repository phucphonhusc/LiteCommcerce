using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
    public interface ICustomerDAL
    {
        int Add(Customer data);
        bool Update(Customer data);
        bool Delete(string[] customerIDs);
        Customer Get(string customerID);
        List<Customer> List(int page, int pageSize, string searchValue);
        int Count(string searchValue);
        List<Customer> ListAll();
    }
}

