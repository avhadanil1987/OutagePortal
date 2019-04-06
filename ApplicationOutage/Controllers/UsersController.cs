using ApplicationOutage.Models;
using ApplicationOutage.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ApplicationOutage.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginUser(LoginModel LoginUser)
        {
            UserManager userManager = new UserManager();
            if (userManager.IsUserActivated(LoginUser))
            {
                FormsAuthentication.SetAuthCookie(LoginUser.UserEmail, false);
              string s =  HttpContext.User.Identity.Name;
               return RedirectToAction("Index", "Outage");
            }
            ViewBag.Message = "Incorrect User Email or Password or your Account not yet activated. Contact to your application administrator.";
               return View("Login");
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterUser(UsersViewModel user)
        {
            if (ModelState.IsValid)
            {
                UserManager userMgr = new UserManager();
                userMgr.RegisterUser(user);
                return RedirectToAction("Login");
            }
            return View("Register");
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        public ActionResult ForgotPassword()
        {
            return View("ResetPassword");
        }

        [HttpPost]
        public ActionResult SetUserPassword([Bind(Exclude = "Password,ConfirmPassword")] ForgotPasswordModel user)
        {
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");
            if (ModelState.IsValid)
            {
                UserManager userManager = new UserManager();
                if (userManager.ResetPassword(user))
                {
                    return RedirectToAction("Login");
                }
            }
            return View("ForgotPassword");
        }
    }
}