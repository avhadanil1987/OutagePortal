using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApplicationOutage.Models
{
    public class ApplicationManager
    {
        public List<Application> GetApplicationList()
        {
            using (ApplicationOutageEntities entities = new ApplicationOutageEntities())
            {
                return entities.Applications.ToList();
            }
        }
    }
}