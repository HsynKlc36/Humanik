using HumanIK.ENTITIES.CustomValidations;
using HumanIK.ENTITIES.Enums;
using Microsoft.AspNetCore.Http;
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
    [Index("TaxNo", IsUnique = true)]
    [Index("MersisNo", IsUnique = true)]
    public class Company : IBaseEntity
    {
        [Key]
        public int CompanyID { get; set; }
        [NotMapped]//veri tabanına kolon olarak eklemez
        [Display(Name = "Logo")]
        [AllowedExtensions(new string[] { ".jpg", ".png" , ".jpeg" })]
        public IFormFile LogoFile { get; set; }
        [Display(Name ="Şirket Adı")]
        public string Name { get; set; }
        [Display(Name = "Şirket Türü")]
        public Title Title { get; set; }
        
        public string Logo { get; set; }
        [Display(Name = "Vergi Dairesi")]
        public string TaxAdministration { get; set; }
        [Display(Name = "Vergi Numarası")]
        public string TaxNo { get; set; }
        [Display(Name = "Mersis No")]
        public string MersisNo { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Kuruluş Tarihi")]
        public DateTime Founded { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Anlaşma Başlangıç Tarihi")]
        public DateTime DealStartDate { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Anlaşma Bitiş Tarihi")]
        public DateTime DealEndDate { get; set; }
        [Display(Name = "Adres")]
        public string Address { get; set; }
        [Display(Name = "Çalışan Sayısı")]
        public int NumberOfEmployees { get; set; }
        [Display(Name = "Telefon Numarası")]
        public string PhoneNumber { get; set; }
        [Display(Name = "E-Posta")]
        public string Email { get; set; }
        [Display(Name = "Sektör")]
        public Sector Sector { get; set; }       
        [Display(Name = "Anlaşma Durumu")]
        public Status DealStatus { get; set; }
        public ICollection<AppUser> Managers { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        [Display(Name = "Durum")]
        public Status Status { get; set; } = Status.Active;
    }
}
