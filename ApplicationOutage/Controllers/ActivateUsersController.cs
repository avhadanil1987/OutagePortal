using ApplicationOutage.Models;
using ApplicationOutage.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using PagedList;
using System.Web;
using System.Web.Mvc;
using System.Net;
using ApplicationOutage.Filters;

namespace ApplicationOutage.Controllers
{

    [AdminAuthorize]
    public class ActivateUsersController : Controller
    {
        // GET: ActivateUsers
        //[Authorize(Users ="SuperAdmin")]
        public ActionResult Index(string searchString, int? page)
        {
            UserManager userManager = new UserManager();
            List<UsersViewModel> users; 
            if (!string.IsNullOrEmpty(searchString))
            {
                page = 1;
                users = userManager.GetRegisteredUsers().Where(x=>x.UserEmail.Contains(searchString)).ToList();
            }
            else
            {
                users = userManager.GetRegisteredUsers().ToList();
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(users.ToPagedList(pageNumber, pageSize));
        }

        // GET: ActivateUsers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserManager userManager = new UserManager();
            UsersViewModel user = userManager.GetUser(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: ActivateUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Exclude = "Password,ConfirmPassword")] UsersViewModel user)
        {
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");
            if (ModelState.IsValid)
            {
                UserManager userManager = new UserManager();

                if (userManager.EditUser(user))
                {
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Edit");
            }
            return View(user);
        }
    }
}