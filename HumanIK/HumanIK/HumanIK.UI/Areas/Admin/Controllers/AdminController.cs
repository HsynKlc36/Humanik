using CloudinaryDotNet;
using FluentValidation;
using FluentValidation.Results;
using FormHelper;
using HumanIK.BUSINESS.Abstract;
using HumanIK.BUSINESS.Concrete;
using HumanIK.ENTITIES;
using HumanIK.ENTITIES.Entities;
using HumanIK.ENTITIES.Enums;
using HumanIK.UI.Areas.Admin.Models;
using HumanIK.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanIK.UI.Areas.AdminArea.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        public AdminController(UserManager<AppUser> userManager)
        {
            _userManager = userManager; // Identity'nin UserManager class'ından instance alınıyor

        }

        public async Task<IActionResult> Index() //Login olan kullanıcının özet bilgileri 
        {
            AppUser currentUser = await _userManager.GetUserAsync(HttpContext.User); // Identity üzerinden login olan kullanıcının bilgileri getiriliyor
            return View(currentUser);
        }

        public async Task<IActionResult> Details() //Login olan kullanıcının tüm bilgileri 
        {
            AppUser currentUser = await _userManager.GetUserAsync(HttpContext.User); // Identity üzerinden login olan kullanıcının bilgileri getiriliyor
            return View(currentUser);
        }

        [HttpGet]
        public async Task<IActionResult> Update()
        {

            AppUser user = await _userManager.GetUserAsync(HttpContext.User); // Identity üzerinden login olan kullanıcının bilgileri getiriliyor
            UpdateViewModel uvm = new UpdateViewModel(); 
            uvm.PersonelPhone = user.PersonalPhone;
            uvm.ProfilePhotoPath = user.ProfilePhoto;
            uvm.Adress = user.Address;
            uvm.ID = user.Id;
            uvm.Name = user.Name;
            uvm.SecondName = user.SecondName;
            uvm.LastName = user.LastName;
            uvm.SecondLastName = user.SecondLastName;

            return View(uvm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateViewModel uvm)
        {
            if (!ModelState.IsValid)
            {
                return View(uvm);
            }
            AppUser user = await _userManager.GetUserAsync(HttpContext.User);
            user.PersonalPhone = uvm.PersonelPhone;
            user.Address = uvm.Adress;
            //Eğer güncelleme sırasında yeni fotoğrag yüklenmediyse eski fotoğraf korunur, yüklendiyse URL güncellenir
            //Static Class olarak oluşturulan resim kaydetme metodu kullanılıyor (Business/Concrete)
            string photoUrl = uvm.ProfilePhoto == null ? uvm.ProfilePhotoPath : await ImageSaver.SaveImage(uvm.ProfilePhoto); 
            user.ProfilePhoto = photoUrl;

            await _userManager.UpdateAsync(user);

            TempData["Message"] = "Bilgileriniz güncellendi.";
            return RedirectToAction("Index");
        }

       
    }
}
