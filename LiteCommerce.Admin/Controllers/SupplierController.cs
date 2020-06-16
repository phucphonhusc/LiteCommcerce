using LiteCommerce.Admin.Codes;
using LiteCommerce.BusinessLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin.Controllers
{
    [Authorize]
    public class SupplierController : Controller
    {
        // GET: Supplier
        /// <summary>
        /// Trang hiển thị: danh sách supplier, các liên kết đến các chức năng liên quan
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int page =1, string searchValue= "")
        {
            var model = new Models.SuppilerPaginationResult()
            {
                Page = page,
                PageSize = AppSetting.DefaultPageSize,
                RowCount = CatalogBLL.Supplier_Count(searchValue),
                Data = CatalogBLL.Supplier_List(page, AppSetting.DefaultPageSize, searchValue),
                SearchValue = searchValue,
            };
            return View(model);
        }
        /// <summary>
        /// add or edit
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns> 
        [HttpGet]
        public ActionResult Input(string id="")
        {
            if(string.IsNullOrEmpty(id))
            {
                ViewBag.Title = "Add new suppliers";
                Supplier newSupplier = new Supplier();
                newSupplier.SupplierID = 0;
                return View(newSupplier);
            }
            else
            {
                ViewBag.Title = "Edit suppliers";
                Supplier editSupplier = CatalogBLL.Supplier_Get(Convert.ToInt32(id));
                if(editSupplier == null)
                {
                    return RedirectToAction("Index");
                }
                return View(editSupplier);
                
            }
        }
        [HttpPost]
        public ActionResult Input( Supplier model)
        {
            try
            {
                //kiem tra tinh hop le 
                if (string.IsNullOrEmpty(model.CompanyName))
                    ModelState.AddModelError("CompanyName", "Company Name is required");
                if (string.IsNullOrEmpty(model.ContactName))
                    ModelState.AddModelError("ContactName", "ContactName is required");
                if (string.IsNullOrEmpty(model.ContactTitle))
                    ModelState.AddModelError("ContactName", "ContactName is required");
                if (string.IsNullOrEmpty(model.Address))
                    ModelState.AddModelError("Address", "Address is required");
                if (string.IsNullOrEmpty(model.City))
                    ModelState.AddModelError("City", "City is required");
                if (string.IsNullOrEmpty(model.Country))
                    ModelState.AddModelError("Country", "Country is required");
                if (string.IsNullOrEmpty(model.Phone))
                    ModelState.AddModelError("Phone", "Phone is required");

               /* ModelState.AddModelError("asda", "loi 1");*/
                

                if (string.IsNullOrEmpty(model.Fax))
                    model.Fax = "";
                if (string.IsNullOrEmpty(model.HomePage))
                    model.HomePage = "";

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                if (model.SupplierID == 0)
                {
                    int suppplierId = CatalogBLL.Supplier_Add(model);
                    return RedirectToAction("Index");
                }
                else
                {
                    bool updateResult = CatalogBLL.Supplier_Update(model);
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
        public ActionResult Delete(string method = "", int[] supplierIDs = null)
        {
            if(supplierIDs != null)
            {
                CatalogBLL.Supplier_Delete(supplierIDs);
            }
            return RedirectToAction("Index");
        }
    }
}