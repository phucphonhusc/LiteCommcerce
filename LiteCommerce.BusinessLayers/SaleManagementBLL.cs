using LiteCommerce.DataLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.BusinessLayers
{
    public static class SaleManagementBLL
    {
        private static IOrderDAL OrderDB { get; set; }
        private static IOrderDetailDAL OrderDetailDB { get; set; }
        public static void Initialize(string connectionString)
        {
            OrderDB = new DataLayers.SqlServer.OrderDAL(connectionString);
            OrderDetailDB = new DataLayers.SqlServer.OrderDetailDAL(connectionString);
        }
        public static List<Order> Order_List(int page, int pageSize, string searchValue)
        {
            return OrderDB.List(page, pageSize, searchValue);
        }
        public static int Order_Count(string searchValue)
        {
            return OrderDB.Count(searchValue);
        }
        public static Order Order_Get(int orderID)
        {
            return OrderDB.Get(orderID);
        }
        public static List<OrderDetail> OrderDetail_Get(int orderID)
        {
            return OrderDetailDB.GetDetail(orderID);
        }
    }
}
