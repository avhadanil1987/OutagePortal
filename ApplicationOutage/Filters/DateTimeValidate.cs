using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ApplicationOutage.Filters
{
    public sealed class DateValidationAttribute : ValidationAttribute
    {
        private DateTime _date;
        public DateValidationAttribute(DateTime date)
        {
            _date = date;
        }
        public override bool IsValid(Object value)
        {
            DateTime dateValue;
            var date = DateTime.TryParse(value.ToString(), out dateValue);
            return dateValue < _date;
        }
    }
}
