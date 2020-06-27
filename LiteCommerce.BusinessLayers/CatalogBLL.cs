using LiteCommerce.DataLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.BusinessLayers
{
    /// <summary>
    /// 
    /// </summary>
    public static class CatalogBLL
    {
        private static ISupplierDAL SupplierDB { get; set; }
        private static ICustomerDAL CustomerDB { get; set; }
        private static IShipperDAL ShipperDB { get; set; }
        private static ICategoryDAL CategoryDB { get; set; }
        private static IProductDAL ProductDB { get; set; }
        private static IProductAttributesDAL ProductAttributeDB { get; set; }
       
        private static ICountryDAL CountryDB { get; set; }
        /// <summary>
        /// Hàm này phải được gọi để khởi tạo các chức năng tác nghiệp
        /// </summary>
        /// <param name="connectionString"></param>
        public static void Initialize(string connectionString)
        {
            SupplierDB = new DataLayers.SqlServer.SupplierDAL(connectionString);
            CustomerDB = new DataLayers.SqlServer.CustomerDAL(connectionString);
            ShipperDB = new DataLayers.SqlServer.ShipperDAL(connectionString);
            CategoryDB = new DataLayers.SqlServer.CategoryDAL(connectionString);
           
            ProductDB = new DataLayers.SqlServer.ProductDAL(connectionString);
            CountryDB = new DataLayers.SqlServer.CountryDAL(connectionString);
            ProductAttributeDB = new DataLayers.SqlServer.ProductAttributeDAL(connectionString);

        }
        /// <summary>
        /// Hiển thị danh sách nhà cung cấp
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
       
        public static List<Supplier> Supplier_List(int page, int pageSize, string searchValue)
        {         
            return SupplierDB.List(page, pageSize, searchValue);
        }
        public static Supplier Supplier_Get(int supplierID)
        {
            return SupplierDB.Get(supplierID);
        }
        public static int Supplier_Add(Supplier data)
        {
            return SupplierDB.Add(data);
        }
        public static bool Supplier_Delete(int[] supplierIDs)
        {
            return SupplierDB.Delete(supplierIDs);
        }
        public static bool Supplier_Update(Supplier data)
        {
            return SupplierDB.Update(data);
        }
        
        public static List<Customer> Customer_List(int page, int pageSize, string searchValue)
        {
            return CustomerDB.List(page, pageSize, searchValue);
        }
        
        public static Customer Customer_Get(string customerID)
        {
            return CustomerDB.Get(customerID);
        }
        public static int Customer_Add(Customer data)
        {
            return CustomerDB.Add(data);
        }
        public static bool Customer_Delete(string[] customerIDs)
        {
            return CustomerDB.Delete(customerIDs);
        }
        public static bool Customer_Update(Customer data)
        {
            return CustomerDB.Update(data);
        }


        public static List<Shipper> Shipper_List(string searchValue)
        {
            return ShipperDB.List(searchValue);
        }
        public static Shipper Shipper_Get(int shipperID)
        {
            return ShipperDB.Get(shipperID);
        }
        public static int Shipper_Add(Shipper data)
        {
            return ShipperDB.Add(data);
        }
        public static bool Shipper_Delete(int[] shipperIDs)
        {
            return ShipperDB.Delete(shipperIDs);
        }
        public static bool Shipper_Update(Shipper data)
        {
            return ShipperDB.Update(data);
        }

        public static List<Category> Category_List(string searchValue)
        {
            return CategoryDB.List(searchValue);
        }
        public static Category Category_Get(int categoryID)
        {
            return CategoryDB.Get(categoryID);
        }
        public static int Category_Add(Category data)
        {
            return CategoryDB.Add(data);
        }
        public static bool Category_Delete(int[] categoryIDs)
        {
            return CategoryDB.Delete(categoryIDs);
        }
        public static bool Category_Update(Category data)
        {
            return CategoryDB.Update(data);
        }

       
        
        
        
        public static int Supplier_Count(string searchValue)
        {
            return SupplierDB.Count(searchValue);
        }
        public static int Customer_Count(string searchValue)
        {
            return CustomerDB.Count(searchValue);
        }
        public static int Shipper_Count(string searchValue)
        {
            return ShipperDB.Count(searchValue);
        }
        public static int Category_Count(string searchValue)
        {
            return CategoryDB.Count(searchValue);
        }

        

        public static List<Product> Product_List(int page, int pageSize, string searchValue, int category, int supplier)
        {
            return ProductDB.List(page, pageSize, searchValue,category,supplier);
        }
        public static int Product_Count(string searchValue, int category, int supplier)
        {
            return ProductDB.Count(searchValue, category, supplier);
        }
        public static Product Product_Get(int productId)
        {
            return ProductDB.Get(productId);
        }
        public static int Product_Add(Product data)
        {
            return ProductDB.Add(data);
        }
        public static bool Product_Delete(int[] productIDs)
        {
            return ProductDB.Delete(productIDs);
        }
        public static bool Product_Update(Product data)
        {
            return ProductDB.Update(data);
        }

        public static List<Country> Country_List()
        {
            return CountryDB.List();
        }

        public static List<ProductAttributes> ProductAttribute_Get(int productID)
        {
            return ProductAttributeDB.Get(productID);
        }
        public static ProductAttributes ProductAttribute_GetAttr(int AttributeID)
        {
            return ProductAttributeDB.GetAttribute(AttributeID);
        }
        public static int ProductAttribute_Add(ProductAttributes data)
        {
            return ProductAttributeDB.Add(data);
        }
        public static bool ProductAttribute_Delete(int[] attributeIDs)
        {
            return ProductAttributeDB.Delete(attributeIDs);
        }
        public static bool ProductAttribute_Update(ProductAttributes data)
        {
            return ProductAttributeDB.Update(data);
        }


        public static List<Supplier> Supplier_ListAll()
        {
            return SupplierDB.ListAll();
        }
        public static List<Shipper> Shipper_ListAll()
        {
            return ShipperDB.ListAll();
        }
        public static List<Category> Category_ListAll()
        {
            return CategoryDB.ListAll();
        }
        public static List<Product> Product_ListAll()
        {
            return ProductDB.ListAll();
        }
        public static List<Customer> Customer_ListAll()
        {
            return CustomerDB.ListAll();
        }
    }
}
