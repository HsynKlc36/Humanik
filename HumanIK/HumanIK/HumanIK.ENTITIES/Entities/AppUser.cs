using HumanIK.ENTITIES.CustomValidations;
using HumanIK.ENTITIES.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanIK.ENTITIES.Entities
{
    [Index("CitizenId", IsUnique = true)]
    public class AppUser : IdentityUser<int>, IBaseEntity
    {
        // BaseEntityden gelen propertyler
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        [Display(Name ="Durum")]
        public Status Status { get; set; } = Status.Active;

        [RegularExpression(@"^[1-9]{1}[0-9]{9}[02468]{1}$")]
        [StringLength(11, ErrorMessage = "TC kimlik numarası 11 haneli olmalıdır")]
        [Display(Name = "T.C. Kimlik Numarası")]
        public string CitizenId { get; set; }
        [Display(Name = "Profil Fotoğrafı")]
        public string ProfilePhoto { get; set; }
        //[RegularExpression()]
        [Display(Name = "Adı")]
        [RegularExpression(@"^[a-zA-ZıüöçşğİÜÖÇŞĞ]+$", ErrorMessage = "Lütfen yalnızca harf girin.")]
        public string Name { get; set; }
        [Display(Name = "İkinci Adı")]
        [RegularExpression(@"^[a-zA-ZıüöçşğİÜÖÇŞĞ]+$", ErrorMessage = "Lütfen yalnızca harf girin.")]
        public string SecondName { get; set; }
        [Display(Name = "Soyadı")]
        [RegularExpression(@"^[a-zA-ZıüöçşğİÜÖÇŞĞ]+$", ErrorMessage = "Lütfen yalnızca harf girin.")]
        public string LastName { get; set; }
        [Display(Name = "İkinci Soyadı")]
        [RegularExpression(@"^[a-zA-ZıüöçşğİÜÖÇŞĞ]+$", ErrorMessage = "Lütfen yalnızca harf girin.")]
        public string SecondLastName { get; set; }
        [Display(Name = "Cinsiyet")]
        public Gender Gender { get; set; }
        [Display(Name = "Mesleği")]
        public Job Job { get; set; }
        [Display(Name = "Departmanı")]
        public Department Department { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Doğum Tarihi")]
        public DateTime BirthDate { get; set; }
        [Display(Name = "İşe Başlangıç Tarihi")]
        public DateTime StartDate { get; set; }
        [Display(Name = "Adres")]
        public string Address { get; set; }

        [Phone]
        [Display(Name = "Telefon Numarası")]
        public override string PhoneNumber { get; set; }

        [Phone]
        [Display(Name = "Kişisel Telefon Numarası")]
        public string PersonalPhone { get; set; }
        public bool IsEverLoggedIn { get; set; }

        [ForeignKey("Company")]
        public int CompanyID { get; set; }
        public Company Company { get; set; }
        [Display(Name = "İşten Çıkış Tarihi")]
        public DateTime? DateOfQuit { get; set; }
        public int? AnnualRightOfVacation { get; set; } = 14;
        public int? Salary { get; set; }
        public CurrencyUnit? SalaryCurrencyUnit { get; set; }

        public List<Advance> Advances { get; set; }

        public List<Expense> Expenses { get; set; }

        public List<Permission> Permissions { get; set; }


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
                lastNameShort = SecondLastName[0].ToString() + ".";
            }
            return string.Join(" ", Name, nameShort, LastName, lastNameShort);
        }
        public string GetFullName()
        {
            return string.Join(" ", Name, SecondName, LastName, SecondLastName);
        }
    }
}
