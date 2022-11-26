using HumanIK.BUSINESS.Abstract;
using HumanIK.ENTITIES.Entities;
using HumanIK.ENTITIES.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanIK.UI.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = "admin, manager")]//admin kalkacak!
    public class AdvanceController : Controller
    {
        private readonly IGenericService<Advance> _advanceService;
        private readonly UserManager<AppUser> _userManager;

        public AdvanceController(IGenericService<Advance> advanceService, UserManager<AppUser> userManager)
        {
            _advanceService = advanceService;
            _userManager = userManager;
        }
        public async Task<IActionResult> AdvanceList()
        {
            AppUser user =await _userManager.GetUserAsync(User);
            var users = _userManager.Users;
            var all = _advanceService.GetAll();
            var advances = _advanceService.GetDefault(x => x.DemandOwner.CompanyID == user.CompanyID)?.OrderByDescending(x=>x.ConfirmationStatus).ThenBy(x=>x.CreateDate);
            // js ile sayfayı yeniliyoruz çünkü yönlendirdiğimiz action'ların view ları yok bu sayfada açılıyor hepsi(AdvanceRefuseRequest,AdvanceConfirmRequest)


            //Admin olarak sorgulandığında dolu gelen DemandOwner property'leri manager olarak sorgulandığında boş geliyor. Bu yüzden manuel olarak dolduruyoruz.

            foreach (var advance in advances)
            {
                var demandOwner = users.FirstOrDefault(x => x.Id == advance.DemandOwnerId);
                advance.DemandOwner = demandOwner;
            }

            return View(advances);
        }

        public IActionResult Refuse(int id)//talep reddetme
        {
            var advance = _advanceService.GetById(id);
            advance.ConfirmationStatus = ConfirmationStatus.No;
            advance.DateOfReply = DateTime.Now;
            _advanceService.Update(advance);
            TempData["Message"] = "Avans talebi reddedildi.";
            return RedirectToAction("AdvanceList");
        }

        public IActionResult Confirm(int id)//talep onaylama 
        {
            var advance = _advanceService.GetById(id);
            advance.ConfirmationStatus = ConfirmationStatus.Yes;
            advance.DateOfReply = DateTime.Now;
            _advanceService.Update(advance);
            TempData["Message"] = "Avans talebi onaylandı.";
            return RedirectToAction("AdvanceList");
        }
    }
}
