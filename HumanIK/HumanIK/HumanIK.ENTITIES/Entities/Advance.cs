using HumanIK.ENTITIES.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanIK.ENTITIES.Entities
{
    public class Advance:IBaseEntity
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
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        [Display(Name = "Durum")]
        public Status Status { get; set; }

        [ForeignKey("DemandOwner")]
        public int DemandOwnerId { get; set; }

        [Display(Name ="Personel")]
        public AppUser DemandOwner { get; set; }
    }
}
