using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
    public interface IOrderDAL
    {
        Order Get(int orderID);
        List<Order> List(int page, int pageSize, string searchValue, string customer);
        int Count(string searchValue, string customer);
    }
}
