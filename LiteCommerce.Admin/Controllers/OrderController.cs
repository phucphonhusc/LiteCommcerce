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
    [Authorize(Roles = WebUserRoles.SALEMAN)]
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult Index(int page = 1, string searchValue = "", string customer="")
        {
            var model = new Models.OrderPaginationResult()
            {
                Page = page,
                PageSize = AppSetting.DefaultPageSize,
                RowCount = SaleManagementBLL.Order_Count(searchValue,customer),
                Data = SaleManagementBLL.Order_List (page, AppSetting.DefaultPageSize, searchValue,customer),
                SearchValue = searchValue,
                customer = customer
           
            };
            ViewData["shipper"] = CatalogBLL.Shipper_ListAll();
            ViewData["customer"] = CatalogBLL.Customer_ListAll();
            ViewData["employee"] = HumanResourceBLL.Employee_ListAll();
            return View(model);
        }
        public ActionResult Detail(string id = "")
        {
            ViewBag.Title = "Detail Order";
            Order order = SaleManagementBLL.Order_Get(Convert.ToInt32(id));
            ViewData["orderdetail"] = SaleManagementBLL.OrderDetail_Get(Convert.ToInt32(id));
            ViewData["product"] = CatalogBLL.Product_ListAll();
            ViewData["shipper"] = CatalogBLL.Shipper_ListAll();
            ViewData["customer"] = CatalogBLL.Customer_ListAll();
            ViewData["employee"] = HumanResourceBLL.Employee_ListAll();
            return View(order);
        }
        public ActionResult Create()
        {
            return View();
        }

    }
}