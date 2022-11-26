using HumanIK.BUSINESS.Abstract;
using HumanIK.BUSINESS.Concrete;
using HumanIK.ENTITIES;
using HumanIK.ENTITIES.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HumanIK.UI.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = "admin, manager")]
    public class ManagerController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public ManagerController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index() // Yönetici özet ekranı
        {
            AppUser currentUser = await _userManager.GetUserAsync(HttpContext.User); // Login olan kullanıcı
            return View(currentUser);
        }

        public async Task<IActionResult> Details() // Yönetici detay ekranı
        {
            AppUser currentUser = await _userManager.GetUserAsync(HttpContext.User);// Login olan kullanıcı
            return View(currentUser);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id) // Yönetici güncelleme ekranı
        {

            AppUser user = await _userManager.GetUserAsync(HttpContext.User); // Login olan kullanıcı
            UpdateViewModel uvm = new UpdateViewModel();
            uvm.PersonelPhone = user.PersonalPhone;
            uvm.ProfilePhotoPath = user.ProfilePhoto;
            uvm.Adress = user.Address;
            uvm.ID = id;
            uvm.Name = user.Name;
            uvm.SecondName = user.SecondName;
            uvm.LastName = user.LastName;
            uvm.SecondLastName = user.SecondLastName;

            return View(uvm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateViewModel uvm) // Yönetici güncelleme
        {
            if (!ModelState.IsValid)
            {
                return View(uvm);
            }
            AppUser user = await _userManager.GetUserAsync(HttpContext.User);
            user.PersonalPhone = uvm.PersonelPhone;
            user.Address = uvm.Adress;
            string photoUrl = uvm.ProfilePhoto == null ? uvm.ProfilePhotoPath : await ImageSaver.SaveImage(uvm.ProfilePhoto);
            user.ProfilePhoto = photoUrl;

            var result = await _userManager.UpdateAsync(user);
            TempData["Message"] = "Bilgileriniz güncellendi.";

            return RedirectToAction("Index");
        }
    }
}
