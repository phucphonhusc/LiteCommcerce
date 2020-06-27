using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
    public interface IProductDAL
    {
        int Add(Product data);
        bool Update(Product data);
        bool Delete(int[] productIDs);
        Product Get(int productID);
        List<Product> List(int page, int pageSize, string searchValue, int category, int supplier);
        int Count(string searchValue, int category, int supplier);
        List<Product> ListAll();
    }
}
