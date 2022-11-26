using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanIK.ENTITIES.Enums
{
    public enum Job
    {
        [Display(Name = "Genel Müdür")]
        GenelMüdür,
        [Display(Name = "Mühendis")]
        Mühendis,
        [Display(Name = "Finans Yöneticisi")]
        Finans,
        [Display(Name = "Satış Yöneticisi")]
        Satis,
        [Display(Name = "Proje Yöneticisi")]
        Proje,
        [Display(Name = "İK Yöneticisi")]
        İnsanKaynaklari,
        [Display(Name = "Pazarlama Yöneticisi")]
        Pazarlama,
        [Display(Name = "Üretim Yöneticisi")]
        Üretim

    }
}
