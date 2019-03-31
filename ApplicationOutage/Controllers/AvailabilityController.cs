using ApplicationOutage.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace ApplicationOutage.Controllers
{
    public class AvailabilityController : Controller
    {
        // GET: Availability
        public ActionResult TotalAvailability(int? Years, int? Months, int? ApplicationID)
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

        [HttpPost]
        public FileStreamResult ExportToExcel(int? Years, int? Months, int? ApplicationID)
        {
            int year = Years.HasValue ? Years.Value : 0;
            int month = Months.HasValue ? Months.Value : 0;
            int applicationId = ApplicationID.HasValue ? ApplicationID.Value : 0;

            var reult = this.CalculateAvailablity(year, month, applicationId);
            if (reult.Any())
            {
                string fileName = DateTime.Now.ToString("MM_dd_yyyy_HH_mm_ss")+"_Availablity.xlsx";
                var serverPath = HttpContext.Server.MapPath("~/Downloads/");
                DirectoryInfo outputDir = new DirectoryInfo(serverPath);
                FileInfo file = new FileInfo(outputDir.FullName+ fileName);

                MemoryStream MS = new MemoryStream();
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    ExcelWorksheet ws = package.Workbook.Worksheets.Add("Availablity");

                    // Set Header.
                    ws.Cells["A1"].Value = "Year";
                    ws.Cells["B1"].Value = "Month";
                    ws.Cells["C1"].Value = "Application Name";
                    ws.Cells["D1"].Value = "Availability";
                    ws.Cells["E1"].Value = "#Outage";
                    ws.Cells["F1"].Value = "Outage (Min)";
                    ws.Cells["G1"].Value = "Goal Availability (Min)";
                    ws.Cells["A1:G1"].Style.Font.Bold = true;
                    ws.Cells[ws.Dimension.Address].AutoFitColumns();
                    
                    // Set color to headers.
                    ws.Cells["A1:G1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Cells["A1:G1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Silver);
                    ws.Cells["A1:G1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    // set style to header cells.
                    ws.Cells["A1"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    ws.Cells["B1"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    ws.Cells["C1"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    ws.Cells["D1"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    ws.Cells["E1"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    ws.Cells["F1"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    ws.Cells["G1"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    // Write data here...
                    int rowCount = 2;
                    int colCount = 0;
                    foreach (var item in reult)
                    {
                        colCount = 1;
                        ws.Cells[rowCount, colCount].Value = item.Year;
                        ws.Cells[rowCount, ++colCount].Value = item.MonthName;
                        ws.Cells[rowCount, ++colCount].Value = item.ApplicationName;
                        ws.Cells[rowCount, ++colCount].Value = item.AvailabilityInPercentage;
                        ws.Cells[rowCount, ++colCount].Value = item.NumberOfOutage;
                        ws.Cells[rowCount, ++colCount].Value = item.Outage;
                        ws.Cells[rowCount, ++colCount].Value = item.GoalAvailability;

                        // set style here.
                        for (int i = 1; i <= colCount; i++)
                        {
                            ws.Cells[rowCount, i].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }
                        rowCount++;
                    }
                    package.SaveAs(MS);
                }
                MS.Position = 0;
               var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                FileStreamResult FSR = new FileStreamResult(MS, contentType);
                FSR.FileDownloadName = fileName;
                return FSR;
            }
            return null;
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
                                NumberOfOutage= monthlyOutage.Count(),
                                Outage = (outageItem.EndDate - outageItem.StartDate).TotalMinutes,
                                Year = item.Year,
                                GoalAvailability = item.Availability
                            });
                        }
                    }
                }

                if (availabilities.Any())
                {
                    var result = availabilities.GroupBy(x => new { x.ApplicationName, x.GoalAvailability, x.Month, x.Year, x.NumberOfOutage }).Select(g => new Availability
                    {
                        ApplicationName = g.Key.ApplicationName,
                        GoalAvailability = g.Key.GoalAvailability,
                        MonthName = Convert.ToString((Months)Enum.ToObject(typeof(Months), g.Key.Month)),
                        Year = g.Key.Year,
                        NumberOfOutage = g.GroupBy(x => x.ApplicationId).Select(y => y.Count()).FirstOrDefault(),
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
