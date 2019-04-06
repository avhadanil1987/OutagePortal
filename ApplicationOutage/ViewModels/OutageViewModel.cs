using ApplicationOutage.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApplicationOutage.ViewModels
{
    public class OutageViewModel:IValidatableObject
    {
        public int ID { get; set; }
        public int ApplicationID { get; set; }

        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Start Date is required.")]
        public System.DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "End Date is required.")]
        public System.DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Impact is required.")]
        public string Impact { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }
        public string Component { get; set; }

        [Display(Name = "Incident #")]
        [Required(ErrorMessage = "Incident Number is required.")]
        public string IncidentNumber { get; set; }
        [Display(Name = "Application Name")]
        public string ApplicationName { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (StartDate > EndDate)
                results.Add(new ValidationResult("End Date cannot be less than Start Date."));

            return results;
        }
    }
}