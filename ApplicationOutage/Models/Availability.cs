using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApplicationOutage.Models
{
    public class Availability
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; }
        public string ApplicationName { get; set; }
        public int ApplicationId { get; set; }
        public double Outage { get; set; }
        public double GoalAvailability { get; set; }
        public double AvailabilityInPercentage { get; set; }
    }
}