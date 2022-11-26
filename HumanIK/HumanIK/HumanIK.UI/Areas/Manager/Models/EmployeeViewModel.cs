using HumanIK.ENTITIES.Enums;
using HumanIK.UI.Areas.Admin.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace HumanIK.UI.Areas.Manager.Models
{
    public class EmployeeViewModel : ManagerViewModel
    {
        [Display(Name="İşten Çıkış Tarihi")]
        public DateTime? DateOfQuit { get; set; }
        public int AnnualRightOfVacation { get; set; } = 14;
        [Display(Name="Maaş")]
        public int Salary { get; set; }
        [Display(Name = "Para Birimi")]
        [Required(ErrorMessage ="Para birimi boş bırakılamaz.")]
        public CurrencyUnit SalaryCurrencyUnit { get; set; }
    }
}