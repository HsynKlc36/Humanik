using HumanIK.BUSINESS.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanIK.BUSINESS.Concrete
{
    public class AppLevelVariables : IAppLevelVariables
    {
        public DateTime LastCheckedDate { get; set; } = DateTime.Now.Date.AddDays(-1);
    }
}
