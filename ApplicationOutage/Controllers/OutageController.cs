using ApplicationOutage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.IO;
using OfficeOpenXml;

namespace ApplicationOutage.Controllers
{
    public class OutageController : Controller
    {
        // GET: Outage
        public ActionResult Index(string searchString, int? page)
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
        public ActionResult Save([Bind(Include = "ID,ApplicationID,StartDate,EndDate,IncidentNumber,Impact,Description,Component")] Outage outage)
        {
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

        [HttpPost]
        public FileStreamResult ExportToExcel()
        {
            OutageManager outageManager = new OutageManager();
            var result = outageManager.GetOutages();
            if (result.Any())
            {
                string fileName = DateTime.Now.ToString("MM_dd_yyyy_HH_mm_ss") + "_IncidenReport.xlsx";
                var serverPath = HttpContext.Server.MapPath("~/Downloads/");
                DirectoryInfo outputDir = new DirectoryInfo(serverPath);
                FileInfo file = new FileInfo(outputDir.FullName + fileName);

                MemoryStream MS = new MemoryStream();
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    ExcelWorksheet ws = package.Workbook.Worksheets.Add("Availablity");

                    // Set Header.
                    ws.Cells["A1"].Value = "Application Name";
                    ws.Cells["B1"].Value = "Start Date";
                    ws.Cells["C1"].Value = "End Date";
                    ws.Cells["D1"].Value = "Total Outage(Min)";
                    ws.Cells["E1"].Value = "Impact";
                    ws.Cells["F1"].Value = "Incident Number";
                    ws.Cells["G1"].Value = "Description";
                    ws.Cells["H1"].Value = "Component";
                    ws.Cells["A1:H1"].Style.Font.Bold = true;

                    // Set color to headers.
                    ws.Cells["A1:H1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Cells["A1:H1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Silver);
                    ws.Cells["A1:H1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    // set style to header cells.
                    ws.Cells["A1"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    ws.Cells["B1"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    ws.Cells["C1"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    ws.Cells["D1"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    ws.Cells["E1"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    ws.Cells["F1"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    ws.Cells["G1"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    ws.Cells["H1"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    // Write data here...
                    int rowCount = 2;
                    int colCount = 0;
                    foreach (var item in result)
                    {
                        colCount = 1;
                        ws.Cells[rowCount, colCount].Value = item.Application.ApplicationName;
                        ws.Cells[rowCount, ++colCount].Value = item.StartDate;
                        ws.Cells[rowCount, colCount].Style.Numberformat.Format = "dd/MM/yyyy HH:mm";
                        ws.Cells[rowCount, ++colCount].Value = item.EndDate;
                        ws.Cells[rowCount, colCount].Style.Numberformat.Format = "dd/MM/yyyy HH:mm";
                        ws.Cells[rowCount, ++colCount].Value = (item.EndDate-item.StartDate).TotalMinutes;
                        ws.Cells[rowCount, ++colCount].Value = item.Impact;
                        ws.Cells[rowCount, colCount].Style.Numberformat.Format = "#0\\.00%";
                        ws.Cells[rowCount, ++colCount].Value = item.IncidentNumber;
                        ws.Cells[rowCount, ++colCount].Value = item.Description;
                        ws.Cells[rowCount, ++colCount].Value = item.Component;

                        // set style here.
                        for (int i = 1; i <= colCount; i++)
                        {
                            ws.Cells[rowCount, i].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }
                        rowCount++;
                    }
                    ws.Cells[ws.Dimension.Address].AutoFitColumns();
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
    }
}