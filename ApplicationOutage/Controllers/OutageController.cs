using ApplicationOutage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
namespace ApplicationOutage.Controllers
{
    public class OutageController : Controller
    {
        // GET: Outage
        public ActionResult Index(string searchString,int? page)
        {
            OutageManager outageManager = new OutageManager();
            List<Outage> outages;
            if (!string.IsNullOrEmpty(searchString))
            {
                page = 1;
                outages = outageManager.GetOutages().Where(x => !string.IsNullOrEmpty(x.IncidentNumber) && x.IncidentNumber.ToLower().Contains(searchString.ToLower())).ToList();
             }
            else
            {
                outages = outageManager.GetOutages();
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(outages.ToPagedList(pageNumber, pageSize));
            
        }

        public ActionResult Add()
        {
            ApplicationManager applicationManager = new ApplicationManager();
            ViewBag.Applications = applicationManager.GetApplicationList().Select(x => new SelectListItem { Text = x.ApplicationName, Value = x.ID.ToString() });
            ViewBag.Components = Components.GetComponents();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save([Bind(Include = "ID,ApplicationID,StartDate,EndDate,IncidentNumber,Impact,Description,Component")] Outage outage) {
            if (ModelState.IsValid)
            {
                OutageManager outageManager = new OutageManager();
                if (outageManager.AddOutage(outage))
                {
                    return RedirectToAction("Index");
                }
            }
            return View(outage);
        }

        // GET: Outages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OutageManager outageManager = new OutageManager();
            Outage outage = outageManager.GetOutage(id);
            if (outage == null)
            {
                return HttpNotFound();
            }
            return View(outage);
        }

        // GET: Outages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OutageManager outageManager = new OutageManager();
            Outage outage = outageManager.GetOutage(id);
            if (outage == null)
            {
                return HttpNotFound();
            }
            ApplicationManager appManager = new ApplicationManager();
            ViewBag.ApplicationID = new SelectList(appManager.GetApplicationList(), "ID", "ApplicationName", outage.ApplicationID);
            ViewBag.Components = Components.GetComponents();
            return View(outage);
        }

        // POST: Outages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ApplicationID,StartDate,EndDate,IncidentNumber,Impact,Description,Component")] Outage outage)
        {
            if (ModelState.IsValid)
            {
                OutageManager outageManager = new OutageManager();

                if (outageManager.EditOutage(outage))
                {
                    return RedirectToAction("Index");
                }


                return RedirectToAction("Index");
            }
            // ViewBag.ApplicationID = new SelectList(db.Applications, "ID", "ApplicationName", outage.ApplicationID);
            return View(outage);
        }

        // GET: Outages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OutageManager outageManager = new OutageManager();
            Outage outage = outageManager.GetOutage(id);
            if (outage == null)
            {
                return HttpNotFound();
            }
            return View(outage);
        }

        // POST: Outages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OutageManager outageManager = new OutageManager();
            outageManager.DeleteOutage(id);
            return RedirectToAction("Index");
        }
    }
}