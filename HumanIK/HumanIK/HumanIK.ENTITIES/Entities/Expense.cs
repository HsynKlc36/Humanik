using HumanIK.ENTITIES.CustomValidations;
using HumanIK.ENTITIES.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanIK.ENTITIES.Entities
{
    public class Expense : IBaseEntity
    {
        public int ID { get; set; }
        [NotMapped, Display(Name = "Belge")]
        [AllowedExtensions(new string[] { ".jpg",".pdf",".png", ".jpeg" })]
        public IFormFile ExpenseFile { get; set; }
      
        public string ExpenseFilePath { get; set; }
        [Display(Name = "Miktar")]
        public int Amount { get; set; }
        [Display(Name = "Cevaplanma Tarihi")]
        public DateTime? DateOfReply { get; set; }
        [Display(Name = "Para Birimi")]
        public CurrencyUnit CurrencyUnit { get; set; }

        [Display(Name = "Reddetme Nedeni")]
        public string ReasonOfRejection { get; set; }
        [Display(Name = "Açıklama")]
        public string Description { get; set; }
        [Display(Name = "Harcama Türü")]
        public ExpenseType ExpenseType { get; set; }
        [Display(Name = "Talep Tarihi")]
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public Status Status { get; set; }
        [Display(Name = "Onay Durumu")]
        public ConfirmationStatus ConfirmationStatus { get; set; } = ConfirmationStatus.Pending;
        [ForeignKey("DemandOwner")]
        public int ExpenseDemandOwnerId { get; set; }
        public AppUser DemandOwner { get; set; }
    }
}
