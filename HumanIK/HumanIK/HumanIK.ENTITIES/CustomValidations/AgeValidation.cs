using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanIK.ENTITIES.CustomValidations
{
    public class AgeValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            DateTime input = (DateTime)value;
            var age = DateTime.Today.Year - input.Year;

            if (input < new DateTime(1940, 1, 1))
            {
                return new ValidationResult("Doğum tarihi 1940'tan önce olamaz");
            }
            else if (age < 18)
            {
                return new ValidationResult("Çalışanın yaşı 18'den küçük olamaz");
            }
            return ValidationResult.Success;
        }

    }
}
