using LiteCommerce.Admin.Codes;
using LiteCommerce.BusinessLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult Index(int page = 1, string searchValue = "")
        {
            var model = new Models.OrderPaginationResult()
            {
                Page = page,
                PageSize = AppSetting.DefaultPageSize,
                RowCount = SaleManagementBLL.Order_Count(searchValue),
                Data = SaleManagementBLL.Order_List (page, AppSetting.DefaultPageSize, searchValue),
                SearchValue = searchValue,
            };
            ViewData["shipper"] = CatalogBLL.Shipper_List("");
            ViewData["customer"] = CatalogBLL.Customer_List(1, CatalogBLL.Customer_Count(""), "");
            ViewData["employee"] = CatalogBLL.Employee_List(1, CatalogBLL.Employee_Count("", ""), "","");
            return View(model);
        }
        public ActionResult Detail(string id = "")
        {
            ViewBag.Title = "Detail Order";
            Order order = SaleManagementBLL.Order_Get(Convert.ToInt32(id));
            ViewData["orderdetail"] = SaleManagementBLL.OrderDetail_Get(Convert.ToInt32(id));
            ViewData["product"] = CatalogBLL.Product_List(1, CatalogBLL.Product_Count("", 0,0), "",0,0);
            ViewData["shipper"] = CatalogBLL.Shipper_List("");
            ViewData["customer"] = CatalogBLL.Customer_List(1, CatalogBLL.Customer_Count(""), "");
            ViewData["employee"] = CatalogBLL.Employee_List(1, CatalogBLL.Employee_Count("", ""), "","");
            return View(order);
        }
        public ActionResult Create()
        {
            return View();
        }

    }
}