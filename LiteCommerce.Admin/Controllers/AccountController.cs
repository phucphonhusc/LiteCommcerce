using LiteCommerce.BusinessLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LiteCommerce.Admin.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            WebUserData userData = User.GetUserData();
            Account account = AccountBLL.Account_Get(userData.UserID);

            return View(account);
        }
        
        public ActionResult ChangePwd(string oldpass, string newpass, string confirmpass)
        {
           
            WebUserData userData = User.GetUserData();
            Account account = AccountBLL.GetPassByEmail(userData.UserID);
            if(string.IsNullOrEmpty(oldpass))   
            {
                ModelState.AddModelError("old", "Old Password is required");
                return View();
            }
            if (string.IsNullOrEmpty(newpass))
            {
                ModelState.AddModelError("new", "New Password is required");
                return View();
            }
            if (string.IsNullOrEmpty(oldpass))
            {
                ModelState.AddModelError("confirm", "Confirm Password is required");
                return View();
            }
            

            if (string.Equals(account.Password , oldpass))
            {
                if (newpass.Equals(confirmpass))
                {
                    bool updateResult = AccountBLL.UpdatePass(newpass, userData.UserID);
                    if (updateResult)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError("check", "The passwords do not match");
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("oldfalse", "The Old passwords incorrect");
                return View();
            }
            

        }
        public ActionResult SignOut()
        {
            Session.Abandon();
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Login(string email = "", string password = "")
        {
            if (Request.HttpMethod == "GET")
            {
                return View();
            }
            else
            {
                //TODO: Kiểm tra thông tin đăng nhập trong DB
                /* bool account = AccountBLL.CheckLogin(email, password);
                 if (account == true)
                 {
                     FormsAuthentication.SetAuthCookie(email, false);

                     return RedirectToAction("Index", "Dashboard");
                 }
                 else
                 {
                     ModelState.AddModelError("", "Đăng nhập thất bại!");
                     ViewBag.Email = email;
                     return View();
                 }*/
                var userAccount = UserAccountBLL.Authorize(email, password, UserAccountTypes.Employee);
                if(userAccount != null)
                {
                    WebUserData cookieData = new Admin.WebUserData()
                    {
                        UserID = userAccount.UserID,
                        FullName = userAccount.FullName,
                        GroupName = WebUserRoles.STAFF,
                        LoginTime = DateTime.Now,
                        SessionID = Session.SessionID,
                        ClientIP = Request.UserHostAddress,
                        Photo = userAccount.Photo
                    };
                    FormsAuthentication.SetAuthCookie(cookieData.ToCookieString(), false);
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    ModelState.AddModelError("", "Đăng nhập thất bại!");
                    ViewBag.Email = email;
                    
                    return View();

                }
            }
        }
       
        [HttpPost]
        public ActionResult UpdateProfile(Account model, HttpPostedFileBase uploadPhoto)
        {
            try
            {
                if (uploadPhoto != null)
                {
                    string filePath = Path.Combine(Server.MapPath("~/Images"), uploadPhoto.FileName);
                    model.PhotoPath = uploadPhoto.FileName;
                    uploadPhoto.SaveAs(filePath);
                    
                    bool updateResult = AccountBLL.UpdateProfile(model);
                    return RedirectToAction("Index");
                   
                }
                else
                {
                    var getEmployee = HumanResourceBLL.Employee_Get(model.EmployeeID);
                    model.PhotoPath = getEmployee.PhotoPath;
                    bool updateResult = AccountBLL.UpdateProfile(model);
                    return RedirectToAction("Index");
                }
                
                
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message + ":" + ex.StackTrace);
                return View(model);
            }
        }


        [AllowAnonymous]
        public ActionResult ForgotPwd()
        {
            return View();
        }

    }
}