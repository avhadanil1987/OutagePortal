using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApplicationOutage.Models
{
    public class AvailabilityJSON
    {
        public double TotalAvailability { get; set; }
        public List<JsonDef> JsonData { get; set; }
        public List<Availability> availabilities { get; set; }
    }

    public class JsonDef
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public enum Months:int
        {
            January=1,
            February,
            March,
            April,
            May,
            Jun,
            July,
            August,
            September,
            Octomber,
            November,
            December

    }
}