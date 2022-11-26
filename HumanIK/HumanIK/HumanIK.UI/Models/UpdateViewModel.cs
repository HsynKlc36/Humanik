using HumanIK.ENTITIES.CustomValidations;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanIK.ENTITIES
{
    public class UpdateViewModel
    {
        [Display(Name = "Profil Fotoğrafı")]
        [AllowedExtensions(new string[] { ".jpg", ".png", ".jpeg" })]
        public IFormFile ProfilePhoto { get; set; }
        
        [Display(Name = "Adres")]
        [MaxLength(250)]
        public string Adress { get; set; }
        [Display(Name = "Kişisel Telefon Numarası")]
        [Required]
        //[PhoneNumberValidation]
        //[Phone, StringLength(11, ErrorMessage = "Telefon numarası 11 haneli olmalıdır")]
        
        [StringLength(14, MinimumLength =14,ErrorMessage = "Telefon numarası 10 haneden oluşmalıdır.")]//çzel karakterleri de kabul ettiğinden dolayı 14 yapıldı ()--
        public string PersonelPhone { get; set; }
        public string ProfilePhotoPath { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public string SecondLastName { get; set; }

        public string GetFullNameShort()
        {
            string lastNameShort = string.Empty;
            string nameShort = string.Empty;
            if (!string.IsNullOrEmpty(SecondName))
            {
                nameShort = Name[0].ToString() + ".";
            }
            if (!string.IsNullOrEmpty(SecondLastName))
            {
                lastNameShort = LastName[0].ToString() + ".";
            }
            return string.Join(" ", Name, nameShort, LastName, lastNameShort);
        }
        public string GetFullName()
        {
            return string.Join(" ", Name, SecondName, LastName, SecondLastName);
        }
    }
}
