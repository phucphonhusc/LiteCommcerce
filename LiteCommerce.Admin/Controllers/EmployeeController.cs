using LiteCommerce.Admin.Codes;
using LiteCommerce.BusinessLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult Index(int page = 1, string searchValue = "", string country ="")
        {
            var model = new Models.EmployeePaginationResult()
            {
                Page = page,
                PageSize = AppSetting.DefaultPageSize,
                RowCount = HumanResourceBLL.Employee_Count(searchValue, country),
                Data = HumanResourceBLL.Employee_List(page, AppSetting.DefaultPageSize, searchValue,country),
                SearchValue = searchValue,
                country = country
            };
            ViewData["country"] = CatalogBLL.Country_List();
            return View(model);
        }
        [HttpGet]
        public ActionResult Input(String id = "")
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.Title = "Add new Employee";
                Employee newEmployee = new Employee();
                newEmployee.EmployeeID = 0;
                return View(newEmployee);
            }
            else
            {
                ViewBag.Title = "Edit Employee";
                Employee editEmployee = HumanResourceBLL.Employee_Get(Convert.ToInt32(id));
                if (editEmployee == null)
                {
                    return RedirectToAction("Index");
                }
                return View(editEmployee);

            }
        }
        [HttpPost]
        public ActionResult Input(Employee model, HttpPostedFileBase uploadPhoto)
        {
            try
            {
               
                //kiem tra tinh hop le 
                if (string.IsNullOrEmpty(model.FirstName))
                    ModelState.AddModelError("FirstName", "First Name is required");
                if (string.IsNullOrEmpty(model.LastName))
                    ModelState.AddModelError("LastName", "Last Name is required");
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                string filePath = Path.Combine(Server.MapPath("~/Images"), uploadPhoto.FileName);
                model.PhotoPath = uploadPhoto.FileName;
                uploadPhoto.SaveAs(filePath);
                if (model.EmployeeID == 0)
                {
                   
                    int employeeId = HumanResourceBLL.Employee_Add(model);
                    return RedirectToAction("Index");
                }
                else
                {
                    bool updateResult = HumanResourceBLL.Employee_Update(model);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message + ":" + ex.StackTrace);
                return View(model);
            }
        }
        public ActionResult Delete(string method="", int[] employeeIDs= null)
        {
            if(employeeIDs != null)
            {
                HumanResourceBLL.Employee_Delete(employeeIDs);
            }
            return RedirectToAction("Index");
        }
    }
}