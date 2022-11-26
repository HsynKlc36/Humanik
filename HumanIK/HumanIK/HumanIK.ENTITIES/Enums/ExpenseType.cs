using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanIK.ENTITIES.Enums
{
    public enum ExpenseType
    {
        [Display(Name = "Yol")]
        Road = 1,
        [Display(Name = "Yemek")]
        Food,
        [Display(Name = "Konaklama")]
        Accomodation
    }
}
