using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
    public interface IShipperDAL
    {
        int Add(Shipper data);
        bool Update(Shipper data);
        bool Delete(int[] shipperIDs);
        Shipper Get(int shipperID);
        List<Shipper> List(string searchValue);
        int Count(string searchValue);
        List<Shipper> ListAll();

    }
}
