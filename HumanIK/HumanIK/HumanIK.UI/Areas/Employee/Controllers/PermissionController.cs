using FluentValidation;
using FluentValidation.Results;
using HumanIK.BUSINESS.Abstract;
using HumanIK.ENTITIES.Entities;
using HumanIK.ENTITIES.Enums;
using HumanIK.UI.Areas.Employee.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanIK.UI.Areas.Employee.Controllers
{
    [Area("Employee")]
    [Authorize(Roles = "admin, employee")]
    public class PermissionController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IGenericService<Permission> _permissionService;
        private readonly IValidator<PermissionViewModel> _validator;

        public PermissionController(UserManager<AppUser> userManager, IGenericService<Permission> permissionService, IValidator<PermissionViewModel> validator)
        {
            _userManager = userManager;
            _permissionService = permissionService;
            _validator = validator;
        }

        public async Task<IActionResult> List()
        {
            AppUser user = await _userManager.GetUserAsync(User);
            var permissions = _permissionService.GetDefault(x => x.PermissionDemandOwnerId == user.Id);
            return View(permissions);
        }

        public async Task<IActionResult> Add()
        {
            AppUser user = await _userManager.GetUserAsync(User);
            var annualPermissionsOfUser = _permissionService.GetDefault(x => x.DemandOwner == user && x.ConfirmationStatus == ConfirmationStatus.Yes && x.CreateDate >= DateTime.Now.AddYears(-1));
            var totalAmount = annualPermissionsOfUser.Sum(x => x.NumberOfDays);
            var remainingPermission = user.AnnualRightOfVacation - totalAmount;

            PermissionViewModel vm = new PermissionViewModel()
            {
                DemandOwner = user,
                RemainingPermission = (int)remainingPermission
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Add(PermissionViewModel model)
        {
            AppUser user = await _userManager.GetUserAsync(User);
            model.DemandOwner = user;

            if (model.NumberOfDays > model.RemainingPermission)
            {
                ModelState.AddModelError("NumberOfDays", "Talep edilen gün kalan yıllık izin hakkınızdan fazla olamaz");
                return View(model);
            }

            if (model.PermissionType != PermissionType.AnnualPermit)
                model.NumberOfDays = GetDaysByPermissionType(model.PermissionType);

            model.EndDate = AddBusinessDays(model.StartDate, model.NumberOfDays);
            ModelState.Clear();
            TryValidateModel(model);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ValidationResult validationResult = await _validator.ValidateAsync(model); //Kendi oluşturduğumuz fluent validationlar ile kontrol sağlanıyor
            if (!validationResult.IsValid)
            {
                return View(model);
            }

            Permission permission = new Permission()
            {
                DemandOwner = user,
                PermissionType = model.PermissionType,
                StartDate = model.StartDate,
                EndDate = (DateTime)model.EndDate, // enddate hesaplanıp entitiye aktarılmalı
                NumberOfDays = model.NumberOfDays,
                CreateDate = model.CreateDate,
                Status = Status.Pending
            };


            _permissionService.Add(permission);
            TempData["Message"] = "İzin talebi oluşturuldu.";
            return RedirectToAction("List");

        }

        public async Task<IActionResult> Cancel(int id)
        {
            AppUser user = await _userManager.GetUserAsync(User);
            var permission = _permissionService.GetById(id);
            permission.ConfirmationStatus = ConfirmationStatus.No;
            _permissionService.Update(permission);
            TempData["Message"] = "İzin Talebi İptal Edildi";
            return RedirectToAction("List");
        }

        DateTime AddBusinessDays(DateTime source, int businessDays)
        {
            var dayOfWeek = businessDays < 0
                                ? ((int)source.DayOfWeek - 12) % 7
                                : ((int)source.DayOfWeek + 6) % 7;

            switch (dayOfWeek)
            {
                case 6:
                    businessDays--;
                    break;
                case -6:
                    businessDays++;
                    break;
            }

            return source.AddDays(businessDays + ((businessDays + dayOfWeek) / 5) * 2);
        }

        public int GetDaysByPermissionType(PermissionType permissionType)
        {

            // 1 gün izin
            if (permissionType == PermissionType.BreastFeedingLeave || permissionType == PermissionType.NewJobSearchPermit || permissionType == PermissionType.PeriodicControlPermission)
                return 1;
            // 3 gün izin
            else if (permissionType == PermissionType.MarriagePermission || permissionType == PermissionType.DeathPermission)
                return 3;
            // 5 gün izin
            else if (permissionType == PermissionType.PaternityLeave)
                return 5;
            // 10 gün izin
            else if (permissionType == PermissionType.DisabilityTreatmentLeave)
                return 10;
            // 30 gün izin : Maternity & Military
            else
                return 30;
        }

    }
}
