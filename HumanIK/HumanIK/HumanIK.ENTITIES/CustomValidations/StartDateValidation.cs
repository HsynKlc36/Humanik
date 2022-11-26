using HumanIK.ENTITIES.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanIK.ENTITIES.CustomValidations
{
    public class StartDateValidation : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return true;

            if (((DateTime)value - DateTime.Now).Days > 30)
                return false;

            return true;
        }
    }
}
