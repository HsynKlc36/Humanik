using HumanIK.ENTITIES.Entities;
using HumanIK.ENTITIES.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HumanIK.UI.Areas.Employee.Models
{
    public class PermissionViewModel
    {
        public int ID { get; set; }
        public PermissionType PermissionType { get; set; }
        [Display(Name = "İzin Gün Sayısı")]
        public int NumberOfDays { get; set; }
        [Display(Name = "İzin Bitiş Tarihi")]
        public DateTime? EndDate { get; set; }
        [Display(Name = "İzin Başlangıç Tarihi")]
        public DateTime StartDate { get; set; }
        [Display(Name = "Talep Tarihi")]
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        [Display(Name = "Durum")]
        public Status Status { get; set; }
        [Display(Name = "Onay Durumu")]
        public ConfirmationStatus ConfirmationStatus { get; set; }
        public AppUser DemandOwner { get; set; }
        public int RemainingPermission { get; set; }
    }
}
