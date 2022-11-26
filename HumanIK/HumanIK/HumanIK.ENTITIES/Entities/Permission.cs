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
    public class Permission : IBaseEntity
    {
        public int ID { get; set; }
        [Display(Name = "İzin Türü")]
        public PermissionType PermissionType { get; set; }
        [Display(Name = "İzin Gün Sayısı")]
        public int NumberOfDays { get; set; }
        [Display(Name = "İzin Bitiş Tarihi")]
        public DateTime EndDate { get; set; }
        [Display(Name = "İzin Başlangıç Tarihi")]
        public DateTime StartDate { get; set; }
        [Display(Name = "Cevaplanma Tarihi")]
        public DateTime? DateOfReply { get; set; }

        [Display(Name = "Talep Tarihi")]
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        [Display(Name = "Durum")]
        public Status Status { get; set; }
        [Display(Name = "Onay Durumu")]
        public ConfirmationStatus ConfirmationStatus { get; set; } = ConfirmationStatus.Pending;
        [Display(Name = "Reddetme Nedeni")]
        public string ReasonOfRejection { get; set; }

        [ForeignKey("DemandOwner")]
        public int PermissionDemandOwnerId { get; set; }
        [Display(Name = "Talep Eden")]
        public AppUser DemandOwner { get; set; }
    }
}
