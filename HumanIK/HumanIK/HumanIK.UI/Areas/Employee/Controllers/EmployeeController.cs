using FluentValidation;
using HumanIK.BUSINESS.Abstract;
using HumanIK.BUSINESS.Concrete;
using HumanIK.ENTITIES;
using HumanIK.ENTITIES.Entities;
using HumanIK.UI.Areas.Manager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HumanIK.UI.Areas.Employee.Controllers
{
    [Area("Employee")]
    [Authorize(Roles = "admin, employee")]
    public class EmployeeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IGenericService<Company> _companyService;
        private readonly IValidator<EmployeeViewModel> _validator;

        public EmployeeController(UserManager<AppUser> userManager, IGenericService<Company> companyService, IValidator<EmployeeViewModel> validator)
        {
            _userManager = userManager;
            _companyService = companyService;
            _validator = validator;
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
