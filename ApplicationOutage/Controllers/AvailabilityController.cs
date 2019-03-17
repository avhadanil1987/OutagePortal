using ApplicationOutage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApplicationOutage.Controllers
{
    public class AvailabilityController : Controller
    {
        // GET: Availability
        public ActionResult TotalAvailability()
        {
            List<SelectListItem> years = new List<SelectListItem>();
            int currentYear = DateTime.Now.Year;
            for (int i = -10; i <= 10; i++)
            {
                years.Add(new SelectListItem() { Value = (currentYear + i).ToString(), Text = (currentYear + i).ToString() });
            }
            ViewBag.Years = years;

            ViewBag.Months = Enum.GetValues(typeof(Months)).OfType<Enum>().Select(x => new SelectListItem { Text = Enum.GetName(typeof(Months), x), Value = (Convert.ToInt32(x).ToString()) });

            // get Application list
            ApplicationOutageEntities entities = new ApplicationOutageEntities();
            ViewBag.Applications = entities.Applications.Select(x => new SelectListItem { Text = x.ApplicationName, Value = x.ID.ToString() });
            return View();
        }

        [HttpPost]
        public JsonResult TotalAvailability(int Year, int Month, int applicationId)
        {
            var reult = this.CalculateAvailablity(Year,Month, applicationId);
            return Json(reult, JsonRequestBehavior.AllowGet);
        }

        private List<Availability> CalculateAvailablity(int Year, int Month,int applicationId)
        {
            DateTime startDate = new DateTime(Year, 1, 1, 00, 00, 00);
            DateTime endDate = new DateTime(Year, 12, 31, 23, 59, 59);

            if(Month!=0)
            {
                startDate = new DateTime(Year, Month, 1, 00, 00, 00);
                endDate = new DateTime(Year, Month, DateTime.DaysInMonth(Year, Month), 23, 59, 00);
            }

            List<YearlyAvailability> availablity;
            List<Outage> outage;

            using (ApplicationOutageEntities entities = new ApplicationOutageEntities())
            {
                availablity = entities.YearlyAvailabilities.Where(x => x.Year == Year).ToList();
                if (applicationId != 0)
                {
                    outage = entities.Outages.Include("Application").Where(x => x.StartDate >= startDate && x.EndDate <= endDate && x.ApplicationID==
                    applicationId).ToList();
                }
                else
                {
                    outage = entities.Outages.Include("Application").Where(x => x.StartDate >= startDate && x.EndDate <= endDate).ToList();
                }
            }

            if (availablity != null && availablity.Any() && outage != null && outage.Any())
            {
                List<Availability> availabilities = new List<Availability>();

                foreach (var item in availablity)
                {
                    startDate = new DateTime(item.Year, item.Month, 1, 00, 00, 00);
                    endDate = new DateTime(item.Year, item.Month, DateTime.DaysInMonth(item.Year, item.Month), 23, 59, 59);
                    var monthlyOutage = outage.Where(x => x.StartDate >= startDate && x.EndDate <= endDate).ToList();

                    if (monthlyOutage != null && monthlyOutage.Any())
                    {
                        foreach (var outageItem in monthlyOutage)
                        {
                            availabilities.Add(new Availability()
                            {
                                ApplicationName = outageItem.Application.ApplicationName,
                                Month = item.Month,
                                Outage = (outageItem.EndDate - outageItem.StartDate).TotalMinutes,
                                Year = item.Year,
                                GoalAvailability = item.Availability
                            });
                        }
                    }
                }

                if (availabilities.Any())
                {
                    var result = availabilities.GroupBy(x => new { x.ApplicationName, x.GoalAvailability, x.Month, x.Year }).Select(g => new Availability
                    {
                        ApplicationName = g.Key.ApplicationName,
                        GoalAvailability = g.Key.GoalAvailability,
                        MonthName = Convert.ToString((Months)Enum.ToObject(typeof(Months), g.Key.Month)),
                        Year = g.Key.Year,
                        Outage = g.Sum(x => x.Outage),
                        AvailabilityInPercentage = Math.Round((((g.Key.GoalAvailability - g.Sum(x => x.Outage)) * 100) / g.Key.GoalAvailability), 2)
                    }).ToList();

                    return result;
                }
            }
            return null;
        }
    }
}
