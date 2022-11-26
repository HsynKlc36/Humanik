using FluentValidation;
using HumanIK.BUSINESS.Abstract;
using HumanIK.ENTITIES.Entities;
using HumanIK.ENTITIES.Enums;
using HumanIK.UI.Areas.Employee.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HumanIK.UI.Areas.Employee.Controllers
{
    [Area("Employee")]
    [Authorize(Roles = "admin, employee")]
    public class AdvanceController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IGenericService<Advance> _advanceService;
        private readonly IValidator<AdvanceViewModel> _validator;

        public AdvanceController(UserManager<AppUser> userManager, IGenericService<Advance> advanceService, IValidator<AdvanceViewModel> validator)
        {
            _userManager = userManager;
            _advanceService = advanceService;
            _validator = validator;
        }
        public async Task<IActionResult> Add()
        {
            AppUser user = await _userManager.GetUserAsync(User);
            var advancesOfUser = _advanceService.GetDefault(x => x.DemandOwner == user && x.ConfirmationStatus == ConfirmationStatus.Yes && x.CreateDate >= DateTime.Now.AddYears(-1));
            var totalAmount = advancesOfUser.Sum(x => x.Amount);
            var remainingAdvance = (user.Salary * 2) - totalAmount;

            AdvanceViewModel vm = new AdvanceViewModel()
            {
                DemandOwner = user,
                CurrencyUnit = (CurrencyUnit)user.SalaryCurrencyUnit,
                RemainingAdvance = (int)remainingAdvance
            };
            
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AdvanceViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            vm.CreateDate = DateTime.Now;
            AppUser user = await _userManager.GetUserAsync(User);
            Advance advance = new Advance()
            {
                Amount = vm.Amount,
                CurrencyUnit = vm.CurrencyUnit,
                Description = vm.Description,
                ConfirmationStatus = ConfirmationStatus.Pending,
                CreateDate = vm.CreateDate,
                DemandOwner = user,
                
            };
           
            _advanceService.Add(advance);
            return RedirectToAction("List");
        }

        public async Task<IActionResult> Update(int id)
        {
            AppUser user = await _userManager.GetUserAsync(User);
            var advancesOfUser = _advanceService.GetDefault(x => x.DemandOwner == user && x.ConfirmationStatus == ConfirmationStatus.Yes && x.CreateDate >= DateTime.Now.AddYears(-1));
            var totalAmount = advancesOfUser.Sum(x => x.Amount);
            var remainingAdvance = (user.Salary * 2) - totalAmount;
            var advance = _advanceService.GetById(id);

            AdvanceViewModel vm = new AdvanceViewModel()
            {
                Amount = advance.Amount,
                Description = advance.Description,
                DemandOwner = user,
                CurrencyUnit = (CurrencyUnit)user.SalaryCurrencyUnit,
                RemainingAdvance = (int)remainingAdvance
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(AdvanceViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            AppUser user = await _userManager.GetUserAsync(User);
            var advance = _advanceService.GetById(vm.ID);

            advance.Amount = vm.Amount;
            advance.Description = vm.Description;
            advance.CreateDate = vm.CreateDate;
            advance.DemandOwner = user;

            _advanceService.Update(advance);
            TempData["Message"] = "Avans Güncellendi";
            return RedirectToAction("List");
        }

        public async Task<IActionResult> List()
        {
            AppUser user = await _userManager.GetUserAsync(User);
            var advancesOfUser = _advanceService.GetDefault(x => x.DemandOwner == user);

            return View(advancesOfUser);
        }

        public async Task<IActionResult> Cancel(int id)
        {
            AppUser user = await _userManager.GetUserAsync(User);
            var advance = _advanceService.GetById(id);
            advance.ConfirmationStatus = ConfirmationStatus.No;
            _advanceService.Update(advance);
            TempData["Message"] = "Avans Talebi İptal Edildi";
            return RedirectToAction("List");
        }
    }
}
