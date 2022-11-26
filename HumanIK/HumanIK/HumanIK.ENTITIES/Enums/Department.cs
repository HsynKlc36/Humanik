using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanIK.ENTITIES.Enums
{
    public enum Department
    {
        Yazılım = 1,
        [Display(Name = "İnsan Kaynakları")]
        İnsanKaynakları,
        Bilisim,
        Yönetim,
        Pazarlama,
        Finans,
        Muhasebe,
        Hukuk,
        [Display(Name = "AR-GE")]
        ARGE
    }
}
