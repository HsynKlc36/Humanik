using HumanIK.BUSINESS.Abstract;
using HumanIK.ENTITIES.Entities;
using HumanIK.ENTITIES.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HumanIK.UI.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = "admin,manager")]
    public class PermissionController : Controller
    {
        private readonly IGenericService<Permission> _permissionService;
        private readonly UserManager<AppUser> _userManager;

        public PermissionController(IGenericService<Permission> permissionService, UserManager<AppUser> userManager)
        {
            _permissionService = permissionService;
            _userManager = userManager;
        }
        public async Task<IActionResult> List()
        {
            AppUser user = await _userManager.GetUserAsync(User);
            var users = _userManager.Users;
            var all = _permissionService.GetAll();
            var permissions = _permissionService.GetDefault(x => x.DemandOwner.CompanyID == user.CompanyID)?.OrderByDescending(x => x.ConfirmationStatus).ThenBy(x => x.CreateDate);
            // js ile sayfayı yeniliyoruz çünkü yönlendirdiğimiz action'ların view ları yok bu sayfada açılıyor hepsi(AdvanceRefuseRequest,AdvanceConfirmRequest)


            //Admin olarak sorgulandığında dolu gelen DemandOwner property'leri manager olarak sorgulandığında boş geliyor. Bu yüzden manuel olarak dolduruyoruz.

            foreach (var permission in permissions)
            {
                var demandOwner = users.FirstOrDefault(x => x.Id == permission.PermissionDemandOwnerId);
                permission.DemandOwner = demandOwner;
            }

            return View(permissions);
        }

        public IActionResult Refuse(int id, string demandOwner)//talep reddetme
        {
            var permission = _permissionService.GetById(id);
            ViewBag.DemandOwner = demandOwner;
            return View(permission);
        }

        public IActionResult ConfirmRefusing(int id)
        {
            var permission = _permissionService.GetById(id);
            permission.ConfirmationStatus = ConfirmationStatus.No;
            permission.DateOfReply = DateTime.Now;
            _permissionService.Update(permission);
            TempData["Message"] = "İzin talebi reddedildi.";
            return RedirectToAction("List");
        }

        public IActionResult Confirm(int id)//talep onaylama 
        {
            var permission = _permissionService.GetById(id);
            permission.ConfirmationStatus = ConfirmationStatus.Yes;
            permission.DateOfReply = DateTime.Now;
            _permissionService.Update(permission);
            TempData["Message"] = "İzin talebi onaylandı.";
            return RedirectToAction("List");
        }

        

    }
}
