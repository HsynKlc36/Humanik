using HumanIK.ENTITIES.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanIK.ENTITIES.Entities
{
    public class AppRole : IdentityRole<int>, IBaseEntity
    {
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public Status Status { get; set; } = Status.Active;

        // Rol adı id si IdentityRole den geliyor. Ekstra bir ekleme yapmaya gerek yok.

    }
}
