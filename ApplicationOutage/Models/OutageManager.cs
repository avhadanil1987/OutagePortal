using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ApplicationOutage.Models
{
    public class OutageManager
    {
        public bool AddOutage(Outage outage)
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
                    entities.Outages.Add(outage);
                }
                entities.SaveChanges();
                return true;
            }
        }

        public bool EditOutage(Outage outage)
        {

            using (ApplicationOutageEntities entities = new ApplicationOutageEntities())
            {
                entities.Entry(outage).State = EntityState.Modified;
                entities.SaveChanges();
                if (outage.StartDate.Month != outage.EndDate.Month)
                {
                    Outage outageDelete = entities.Outages.Find(outage.ID);
                    entities.Outages.Remove(outage);
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
                    entities.Outages.Add(outage);
                }
                entities.SaveChanges();
                return true;
            }
        }

        public Outage GetOutage(int? id)
        {
            ApplicationOutageEntities entities = new ApplicationOutageEntities();
            Outage outage = entities.Outages.Find(id);
            return outage;
        }

        public Outage DeleteOutage(int id)
        {
            ApplicationOutageEntities entities = new ApplicationOutageEntities();
            Outage outage = entities.Outages.Find(id);
            entities.Outages.Remove(outage);
            entities.SaveChanges();
            return outage;
        }

        public List<Outage> GetOutages()
        {
            using (ApplicationOutageEntities entities = new ApplicationOutageEntities())
            {
                return entities.Outages.Include("Application").ToList();
            }
        }
    }
}