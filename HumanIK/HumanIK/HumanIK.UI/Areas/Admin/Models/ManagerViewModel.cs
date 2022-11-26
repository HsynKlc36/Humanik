using HumanIK.ENTITIES.CustomValidations;
using HumanIK.ENTITIES.Entities;
using HumanIK.ENTITIES.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace HumanIK.UI.Areas.Admin.Models
{
    public class ManagerViewModel
    {
        public int ID { get; set; }
        [Display(Name = "Profil Fotoğrafı")]
        [AllowedExtensions(new string[] { ".jpg", ".png", ".jpeg" })]
        public IFormFile ProfilePhoto { get; set; }
        public string ProfilPhotoPath { get; set; }

        [Display(Name = "T.C. Kimlik Numarası")]
        [StringLength(11, ErrorMessage = "TC kimlik numarası 11 haneli olmalıdır")]
        public string CitizenId { get; set; }

        [Display(Name = "Adı")]
        [RegularExpression(@"^[a-zA-ZıüöçşğİÜÖÇŞĞ]+$", ErrorMessage = "Lütfen yalnızca harf girin.")]
        [MaxLength(15, ErrorMessage = "En fazla {1} karakter kullanabilirsiniz.")]
        public string Name { get; set; }
        [Display(Name = "İkinci Adı")]
        [RegularExpression(@"^[a-zA-ZıüöçşğİÜÖÇŞĞ]+$", ErrorMessage = "Lütfen yalnızca harf girin.")]
        [MaxLength(15, ErrorMessage = "En fazla {1} karakter kullanabilirsiniz.")]
        public string SecondName { get; set; }
        [Display(Name = "Soyadı")]
        [RegularExpression(@"^[a-zA-ZıüöçşğİÜÖÇŞĞ]+$", ErrorMessage = "Lütfen yalnızca harf girin.")]
        [MaxLength(15, ErrorMessage = "En fazla {1} karakter kullanabilirsiniz.")]
        public string LastName { get; set; }
        [Display(Name = "İkinci Soyadı")]
        [RegularExpression(@"^[a-zA-ZıüöçşğİÜÖÇŞĞ]+$", ErrorMessage = "Lütfen yalnızca harf girin.")]
        [MaxLength(15, ErrorMessage = "En fazla {1} karakter kullanabilirsiniz.")]
        public string SecondLastName { get; set; }
        [Display(Name = "Cinsiyet")]
        public Gender Gender { get; set; }
        [Display(Name = "Mesleği")]
        [Required(ErrorMessage ="Mesleği boş bırakılamaz.")]
        public Job Job { get; set; }
        [Display(Name = "Durum")]
        public Status Status { get; set; } = Status.Active;
        [Display(Name = "Departmanı")]
        public Department Department { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Doğum Tarihi")]
        public DateTime BirthDate { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "İşe Başlangıç Tarihi")]
        public DateTime StartDate { get; set; }
        [Display(Name = "Adres")]
        public string Address { get; set; }

        [Phone, StringLength(9, ErrorMessage = "Kurumsal Telefon numarası 7 hane girilmeli ve rakamlardan oluşmalıdır.")]
        [Display(Name = "Telefon Numarası")]
        public string PhoneNumber { get; set; }

        [Phone, StringLength(14, ErrorMessage = "Kişisel Telefon numarası 11 hane girilmeli ve rakamlardan oluşmalıdır.")]
        [Display(Name = "Kişisel Telefon Numarası")]
        [Required(ErrorMessage ="Personel Numarası boş bırakılamaz.")]
        public string PersonalPhone { get; set; }
        [Required]
        public string  Email { get; set; }

        [Display(Name = "Şirketi")]
        public Company Company { get; set; }
    }
}
