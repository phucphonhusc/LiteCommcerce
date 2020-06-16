using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
    public interface IProductAttributesDAL
    {
        int Add(ProductAttributes data);
        bool Update(ProductAttributes data);
        bool Delete(int[] attributeIDs);
        List<ProductAttributes> Get(int productID);
        ProductAttributes GetAttribute(int productID);
    }
}
