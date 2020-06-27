using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
    public interface ICategoryDAL
    {
        int Add(Category data);
        bool Update(Category data);
        bool Delete(int[] categoryIDs);
        Category Get(int categoryID);
        
        List<Category> List( string searchValue);
        int Count(string searchValue);
        List<Category> ListAll();
    }
}
