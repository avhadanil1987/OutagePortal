using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApplicationOutage.Models
{
    public class Components
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public static List<Components> GetComponents()
        {
            return new List<Components>() { new Components() { Key = "Network", Value = "Network" },
                new Components() { Key = "Application", Value = "Application" },
                new Components() { Key = "System", Value = "System" },
            };
        }
    }
}
