using ApplicationOutage.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ApplicationOutage.Models
{
    public class OutageManager
    {
        public bool AddOutage(OutageViewModel outage)
        {
            using (ApplicationOutageEntities entities = new ApplicationOutageEntities())
            {
                if (outage.StartDate.Month != outage.EndDate.Month)
                {
                    DateTime newEndDate = new DateTime(outage.StartDate.Year, outage.StartDate.Month, (DateTime.DaysInMonth(outage.StartDate.Year, outage.StartDate.Month)), 23, 59, 00);
                    entities.Outages.Add(new Outage()
                    {
                        ApplicationID = outage.ApplicationID,
                        StartDate = outage.StartDate,
                        EndDate = newEndDate,
                        Component = outage.Component,
                        IncidentNumber = outage.IncidentNumber,
                        Impact = outage.Impact,
                        Description = outage.Description
                    });

                    DateTime newStartDate = new DateTime(outage.EndDate.Year, outage.EndDate.Month, 1, 00, 00, 00);
                    entities.Outages.Add(new Outage()
                    {
                        ApplicationID = outage.ApplicationID,
                        StartDate = newStartDate,
                        EndDate = outage.EndDate,
                        Component = outage.Component,
                        IncidentNumber = outage.IncidentNumber,
                        Impact = outage.Impact,
                        Description = outage.Description
                    });
                }
                else
                {
                    entities.Outages.Add(new Outage()
                    {
                        ApplicationID = outage.ApplicationID,
                        StartDate = outage.StartDate,
                        EndDate = outage.EndDate,
                        Component = outage.Component,
                        IncidentNumber = outage.IncidentNumber,
                        Impact = outage.Impact,
                        Description = outage.Description
                    });
                }
                entities.SaveChanges();
                return true;
            }
        }

        public bool EditOutage(OutageViewModel outageData)
        {

            using (ApplicationOutageEntities entities = new ApplicationOutageEntities())
            {
                Outage outage = new Outage()
                {
                    ApplicationID = outageData.ApplicationID,
                    StartDate = outageData.StartDate,
                    EndDate = outageData.EndDate,
                    IncidentNumber = outageData.IncidentNumber,
                    Component = outageData.Component,
                    Description = outageData.Description,
                    ID = outageData.ID,
                    Impact = outageData.Impact
                };

                if (outage.StartDate.Month != outage.EndDate.Month)
                {
                    Outage outageDelete = entities.Outages.Find(outage.ID);
                    entities.Outages.Remove(outageDelete);
                    entities.SaveChanges();

                    DateTime newEndDate = new DateTime(outage.StartDate.Year, outage.StartDate.Month, (DateTime.DaysInMonth(outage.StartDate.Year, outage.StartDate.Month)), 23, 59, 00);
                    entities.Outages.Add(new Outage()
                    {
                        ApplicationID = outage.ApplicationID,
                        StartDate = outage.StartDate,
                        EndDate = newEndDate,
                        Component = outage.Component,
                        IncidentNumber = outage.IncidentNumber,
                        Impact = outage.Impact,
                        Description = outage.Description
                    });

                    DateTime newStartDate = new DateTime(outage.EndDate.Year, outage.EndDate.Month, 1, 00, 00, 00);
                    entities.Outages.Add(new Outage()
                    {
                        ApplicationID = outage.ApplicationID,
                        StartDate = newStartDate,
                        EndDate = outage.EndDate,
                        Component = outage.Component,
                        IncidentNumber = outage.IncidentNumber,
                        Impact = outage.Impact,
                        Description = outage.Description
                    });

                }
                else
                {
                    var oldOutage = entities.Outages.FirstOrDefault(x => x.ID == outage.ID);
                    if (oldOutage != null)
                    {
                        oldOutage.ApplicationID = outageData.ApplicationID;
                        oldOutage.StartDate = outageData.StartDate;
                        oldOutage.EndDate = outageData.EndDate;
                        oldOutage.IncidentNumber = outageData.IncidentNumber;
                        oldOutage.Component = outageData.Component;
                        oldOutage.Description = outageData.Description;
                        oldOutage.ID = outageData.ID;
                        oldOutage.Impact = outageData.Impact;
                        entities.Entry(oldOutage).CurrentValues.SetValues(oldOutage);
                    }
                }
                entities.SaveChanges();
                return true;
            }
        }

        public OutageViewModel GetOutage(int? id)
        {
            ApplicationOutageEntities entities = new ApplicationOutageEntities();
            Outage outage = entities.Outages.FirstOrDefault(x=>x.ID==id);
            if (outage != null)
            {
                return new OutageViewModel()
                {
                    ApplicationID = outage.ApplicationID,
                    Component = outage.Component,
                    Description = outage.Description,
                    EndDate = outage.EndDate,
                    StartDate = outage.StartDate,
                    ID = outage.ID,
                    Impact = outage.Impact,
                    IncidentNumber = outage.IncidentNumber,
                    ApplicationName=outage.Application.ApplicationName
                };
            }
            return null;
        }

        public void DeleteOutage(int id)
        {
            ApplicationOutageEntities entities = new ApplicationOutageEntities();
            Outage outage = entities.Outages.Find(id);
            entities.Outages.Remove(outage);
            entities.SaveChanges();
        }

        public List<Outage> GetOutages()
        {
            using (ApplicationOutageEntities entities = new ApplicationOutageEntities())
            {
                return entities.Outages.Include("Application").ToList();
            }
        }

        public bool GetIncident(string IncidentNumber)
        {
            using (ApplicationOutageEntities entities = new ApplicationOutageEntities())
            {
                var incidentRecord= entities.Outages.FirstOrDefault(x => x.IncidentNumber.ToUpper().Trim().Equals(IncidentNumber.ToUpper().Trim()));
                return incidentRecord == null?false:true;
            }
        }
    }
}