using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanIK.ENTITIES.CustomValidations
{
    public class PhoneNumberValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                decimal val;
                var isNumeric = decimal.TryParse(value.ToString(), out val);

                if (!isNumeric)
                {
                    return new ValidationResult("Telefon numarası 10 haneli olmalı ve rakamlardan oluşmalıdır!");
                }
            }

            return ValidationResult.Success;
        }
    }
}
