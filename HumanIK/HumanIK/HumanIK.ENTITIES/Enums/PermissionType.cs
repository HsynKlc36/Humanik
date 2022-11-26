using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanIK.ENTITIES.Enums
{
    public enum PermissionType
    {
        [Display(Name = "Yıllık İzin")]
        AnnualPermit = 1,

        [Display(Name = "Doğum İzni")]
        MaternityLeave,

        [Display(Name = "Evlenme İzni")]
        MarriagePermission,

        [Display(Name = "Ölüm İzni")]
        DeathPermission,

        [Display(Name = "Babalık İzni")]
        PaternityLeave,

        [Display(Name = "Emzirme İzni")]
        BreastFeedingLeave,

        [Display(Name = "Engelli Çocuk İzni")]
        DisabilityTreatmentLeave,

        [Display(Name = "Periyodik Kontrol İzni")]
        PeriodicControlPermission,

        [Display(Name = "Askerlik İzni")]
        MilitaryDutyLeave,

        [Display(Name = "Yeni İş Arama İzni")]
        NewJobSearchPermit
    }
}
