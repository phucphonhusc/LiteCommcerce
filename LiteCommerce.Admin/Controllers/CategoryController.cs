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
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index(string searchValue= "")
        {
            var model = new Models.CategoryResult()
            {
                RowCount = CatalogBLL.Category_Count(searchValue),
                Data = CatalogBLL.Category_List(searchValue),
                SearchValue = searchValue
            };
            return View(model);
        }
        [HttpGet]
        public ActionResult Input(string id="")
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.Title = "Add new category";
                Category newCategory = new Category();
                newCategory.CategoryID = 0;
                return View(newCategory);
            }
            else
            {
                ViewBag.Title = "Edit category";
                Category editCategory = CatalogBLL.Category_Get(Convert.ToInt32(id));
                if (editCategory == null)
                {
                    return RedirectToAction("Index");
                }
                return View(editCategory);

            }
        }
        [HttpPost]
        public ActionResult Input(Category model)
        {
            try
            {
                //kiem tra tinh hop le 
                if (string.IsNullOrEmpty(model.CategoryName))
                    ModelState.AddModelError("CategoryName", "Category Name is required");
                if (string.IsNullOrEmpty(model.Description))
                    ModelState.AddModelError("Description", "Description is required");
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                if (model.CategoryID == 0)
                {
                    int categoryId = CatalogBLL.Category_Add(model);
                    return RedirectToAction("Index");
                }
                else
                {
                    bool updateResult = CatalogBLL.Category_Update(model);
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
        public ActionResult Delete(string method = "", int[] categoryIDs = null)
        {
            if (categoryIDs != null)
            {
                CatalogBLL.Category_Delete(categoryIDs);
            }
            return RedirectToAction("Index");
        }
    }
}