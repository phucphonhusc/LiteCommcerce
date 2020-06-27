using LiteCommerce.Admin.Codes;
using LiteCommerce.BusinessLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin.Controllers
{
    [Authorize(Roles = WebUserRoles.ADMINISTRATOR)]
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index(int page = 1, string searchValue = "", int category = 0, int supplier = 0)
        {
            var model = new Models.ProductPaginationResult()
            {
                Page = page,
                PageSize = AppSetting.DefaultPageSize,
                RowCount = CatalogBLL.Product_Count(searchValue, category, supplier),
                Data = CatalogBLL.Product_List(page, AppSetting.DefaultPageSize, searchValue, category, supplier),
                SearchValue = searchValue,
                category = category,
                supplier = supplier
            };
            ViewData["category"] = CatalogBLL.Category_ListAll();
            ViewData["supplier"] = CatalogBLL.Supplier_ListAll();
            return View(model);
        }
        [HttpGet]
        public ActionResult Input(string id = "")
        {
            ViewData["category"] = CatalogBLL.Category_ListAll();
            ViewData["supplier"] = CatalogBLL.Supplier_ListAll();

            if (string.IsNullOrEmpty(id))
            {
                ViewBag.Title = "Add new product";
                Product newProduct = new Product();
                newProduct.SupplierID = 0;
                ViewData["attribute"] = CatalogBLL.ProductAttribute_Get(0);
                return View(newProduct);
            }
            else
            {
                ViewBag.Title = "Edit product";
                Product editProduct = CatalogBLL.Product_Get(Convert.ToInt32(id));
                ViewData["attribute"] = CatalogBLL.ProductAttribute_Get(Convert.ToInt32(id));

                if (editProduct == null)
                {
                    return RedirectToAction("Index");
                }
                return View(editProduct);

            }



        }
        [HttpPost]
        public ActionResult Input(Product model, HttpPostedFileBase uploadPhoto)
        {
            ViewData["category"] = CatalogBLL.Category_ListAll();
            ViewData["supplier"] = CatalogBLL.Supplier_ListAll();


            try
            {

                //kiem tra tinh hop le 
                if (string.IsNullOrEmpty(model.ProductName))
                    ModelState.AddModelError("ProductName", "ProductName is required");
                if (string.IsNullOrEmpty(model.Descriptions))
                    ModelState.AddModelError("Descriptions", "Descriptions is required");
                if (string.IsNullOrEmpty(model.QuantityPerUnit))
                    ModelState.AddModelError("QuantityPerUnit", "QuantityPerUnit is required");
                if ((model.UnitPrice) == 0)
                    ModelState.AddModelError("UnitPrice", "UnitPrice is required");
                /*if (string.IsNullOrEmpty(model.AttributeName))
                    model.AttributeName = "";
                if (string.IsNullOrEmpty(model.AttributeValues))
                    model.AttributeValues = "";*/
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                
                if (model.ProductID == 0)
                {
                    if(uploadPhoto != null)
                    {
                        string filePath = Path.Combine(Server.MapPath("~/Images"), uploadPhoto.FileName);
                        model.PhotoPath = uploadPhoto.FileName;
                        uploadPhoto.SaveAs(filePath);
                        int productId = CatalogBLL.Product_Add(model);
                        return RedirectToAction("InputDetail");
                    }
                    else
                    {
                        model.PhotoPath = "";
                        int productId = CatalogBLL.Product_Add(model);
                        return RedirectToAction("InputDetail");
                    }
                    
                }
                else
                {
                    if (uploadPhoto != null)
                    {
                        string filePath = Path.Combine(Server.MapPath("~/Images"), uploadPhoto.FileName);
                        model.PhotoPath = uploadPhoto.FileName;
                        uploadPhoto.SaveAs(filePath);
                        bool updateResult = CatalogBLL.Product_Update(model);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        Product pro = CatalogBLL.Product_Get(model.ProductID);
                        model.PhotoPath = pro.PhotoPath;
                        bool updateResult = CatalogBLL.Product_Update(model);
                        return RedirectToAction("Index");
                    }
                   
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message + ":" + ex.StackTrace);
                return View(model);
            }
        }
        public ActionResult Detail(string id)
        {
            ViewData["category"] = CatalogBLL.Category_ListAll();
            ViewData["supplier"] = CatalogBLL.Supplier_ListAll();
            ViewBag.Title = "Detail product";
            Product product = CatalogBLL.Product_Get(Convert.ToInt32(id));
            ViewData["attribute"] = CatalogBLL.ProductAttribute_Get(Convert.ToInt32(id));
            return View(product);

        }
        [HttpGet]
        public ActionResult InputDetail(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.Title = "Add new attribute";
                ProductAttributes newAttribute = new ProductAttributes();
                newAttribute.AttributeID = 0;
                return View(newAttribute);
            }
            else
            {
                ViewBag.Title = "Edit attribute";
                ProductAttributes editAttribute = CatalogBLL.ProductAttribute_GetAttr(Convert.ToInt32(id));
                if (editAttribute == null)
                {
                    return RedirectToAction("Input");
                }
                return View(editAttribute);

            }
        }
        [HttpPost]
        public ActionResult InputDetail(ProductAttributes model)
        {
            try
            {

                //kiem tra tinh hop le 
                if (string.IsNullOrEmpty(model.AttributeName))
                    ModelState.AddModelError("AttributeName", "AttributeName is required");
                if (string.IsNullOrEmpty(model.AttributeValues))
                    ModelState.AddModelError("AttributeValues", "AttributeValues is required");
                if (model.DisplayOrder==0)
                    ModelState.AddModelError("DisplayOrder", "DisplayOrder is required");
               
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                
                if (model.AttributeID == 0)
                {
                
                    int attributeId = CatalogBLL.ProductAttribute_Add(model);
                    
                    return RedirectToAction("Index");
                }
                else
                {
                 
                    
                    bool updateResult = CatalogBLL.ProductAttribute_Update(model);
                    return RedirectToAction("Input", "Product", new { id = model.ProductID});
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message + ":" + ex.StackTrace);
                return View(model);
            }
        }
        [HttpPost]
        public ActionResult Delete(string method = "", int[] productIDs = null)
        {
            if (productIDs != null)
            {
                CatalogBLL.Product_Delete(productIDs);
            }
            return RedirectToAction("Index");
        }
        public ActionResult DeleteAttribute(string method = "", int[] attributeIDs = null)
        {
            if (attributeIDs != null)
            {
                CatalogBLL.ProductAttribute_Delete(attributeIDs);
            }
            return RedirectToAction("Input");
        }
    }
}