using HumanIK.ENTITIES.Entities;
using HumanIK.ENTITIES.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace HumanIK.UI.Areas.Employee.Models
{
    public class AdvanceViewModel 
    {
        public int ID { get; set; }
        [Display(Name = "Miktar")]
        public int Amount { get; set; }
        [Display(Name = "Para Birimi")]
        public CurrencyUnit CurrencyUnit { get; set; }
        [Display(Name = "Açıklama")]
        public string Description { get; set; }
        [Display(Name = "Cevaplanma Tarihi")]
        public DateTime? DateOfReply { get; set; }
        [Display(Name = "Onay Durumu")]
        public ConfirmationStatus ConfirmationStatus { get; set; } = ConfirmationStatus.Pending;

        [Display(Name = "Talep Tarihi")]
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        [Display(Name = "Durum")]
        public Status Status { get; set; }
        public AppUser DemandOwner { get; set; }
        public int RemainingAdvance { get; set; }
    }
}
