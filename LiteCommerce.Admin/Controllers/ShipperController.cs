using LiteCommerce.BusinessLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin.Controllers
{
    [Authorize(Roles = WebUserRoles.ADMINISTRATOR)]
    public class ShipperController : Controller
    {
        // GET: Shipper
        public ActionResult Index(string searchValue="")
        {
            var model = new Models.ShipperResult()
            {
                RowCount = CatalogBLL.Shipper_Count(searchValue),
                Data = CatalogBLL.Shipper_List(searchValue),
                SearchValue = searchValue
            };
            return View(model);
        }
        [HttpGet]
        public ActionResult Input(String id = "")
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.Title = "Add new shipper";
                Shipper newShipper = new Shipper();
                newShipper.ShipperID = 0;
                return View(newShipper);
            }
            else
            {
                ViewBag.Title = "Edit shipper";
                Shipper editShipper = CatalogBLL.Shipper_Get(Convert.ToInt32(id));
                if (editShipper == null)
                {
                    return RedirectToAction("Index");
                }
                return View(editShipper);

            }
        }
        [HttpPost]
        public ActionResult Input(Shipper model)
        {
            try
            {
                //kiem tra tinh hop le 
                if (string.IsNullOrEmpty(model.CompanyName))
                    ModelState.AddModelError("CompanyName", "Company Name is required");
                if (string.IsNullOrEmpty(model.Phone))
                    ModelState.AddModelError("Phone", "Phone is required");    
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                if (model.ShipperID == 0)
                {
                    int shipperId = CatalogBLL.Shipper_Add(model);
                    return RedirectToAction("Index");
                }
                else
                {
                    bool updateResult = CatalogBLL.Shipper_Update(model);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message + ":" + ex.StackTrace);
                return View(model);
            }
        }
        [HttpPost]
        public ActionResult Delete(string method = "", int[] shipperIDs = null)
        {
            if (shipperIDs != null)
            {
                CatalogBLL.Shipper_Delete(shipperIDs);
            }
            return RedirectToAction("Index");
        }
    }
}