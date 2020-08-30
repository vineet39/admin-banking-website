using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApplication.Attributes
{
    public class ValidDate : ValidationAttribute
    {
        //Date validation referencing https://stackoverflow.com/questions/8844747/restrict-datetime-value-with-data-annotations
        public override bool IsValid(object value)
        {
            DateTime d = Convert.ToDateTime(value);
            return d >= DateTime.UtcNow;
        }
    }
}
