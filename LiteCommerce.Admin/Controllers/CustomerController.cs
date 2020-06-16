using Antlr.Runtime.Tree;
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
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Index(int page = 1, string searchValue= "")
        {
            var model = new Models.CustomerPaginationResult()
            {
                Page = page,
                PageSize = AppSetting.DefaultPageSize,
                RowCount = CatalogBLL.Customer_Count(searchValue),
                Data = CatalogBLL.Customer_List(page, AppSetting.DefaultPageSize, searchValue),
                SearchValue = searchValue
            };
            return View(model);
        }
        [HttpGet]
        public ActionResult Input(string id= "", string method="") 
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.Title = "Add new customer";
                Customer newCustomer = new Customer();
                newCustomer.CustomerID = "";
                ViewBag.method = "Add";
                return View(newCustomer);
            }
            else
            {
                ViewBag.Title = "Edit customer";
                ViewBag.method = "Edit";
                Customer editCustomer = CatalogBLL.Customer_Get((id));
                if (editCustomer == null)
                {
                    
                    return RedirectToAction("Index");
                }
                return View(editCustomer);

            }
        }
        [HttpPost]
        public ActionResult Input(Customer model , string method)
        {
            try
            {
                if (string.IsNullOrEmpty(model.CompanyName))
                    ModelState.AddModelError("CompanyName", "Company Name is required");
                if (string.IsNullOrEmpty(model.Phone))
                    ModelState.AddModelError("Phone", "Phone is required");
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                Customer customer = CatalogBLL.Customer_Get(model.CustomerID);
                if (customer == null)
                {
                    int customerId= CatalogBLL.Customer_Add(model);
                    return RedirectToAction("Index");
                }
                else
                {
                    bool updateResult = CatalogBLL.Customer_Update(model);
                    return RedirectToAction("Index");
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message + ":" + ex.StackTrace);
                return View(model);
            }
        }
        [HttpPost]
        public ActionResult Delete(string method = "", string[] customerIDs = null)
        {
            if(customerIDs != null)
            {
                CatalogBLL.Customer_Delete(customerIDs);
            }
            return RedirectToAction("Index");
        }
    }
}