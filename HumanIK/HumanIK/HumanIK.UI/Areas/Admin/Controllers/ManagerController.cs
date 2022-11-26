using FluentValidation;
using FluentValidation.Results;
using HumanIK.BUSINESS.Abstract;
using HumanIK.BUSINESS.Concrete;
using HumanIK.ENTITIES.Entities;
using HumanIK.ENTITIES.Enums;
using HumanIK.UI.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HumanIK.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class ManagerController : Controller
    {
        private readonly IGenericService<Company> _companyService;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _env;
        private readonly IValidator<ManagerViewModel> _managerValidator;
        private readonly UserManager<AppUser> _userManager;
        private int i = 1;

        public ManagerController(UserManager<AppUser> userManager, IGenericService<Company> companyService, IEmailSender emailSender, IWebHostEnvironment env, IValidator<ManagerViewModel> managerValidator)
        {
            _companyService = companyService;
            _emailSender = emailSender; // Email göndermek üzere yazdığımız class
            _env = env; // Geçerli çalışma ortamı hakkında bilgileri tutar
            _managerValidator = managerValidator;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult Add() // Yönetici ekle ekranı
        {
            var companyList = _companyService.GetAll();
            ViewBag.CompanyList = companyList;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(ManagerViewModel model) // Yönetici ekle
        {
            Company company = _companyService.GetById(model.Company.CompanyID); // Model'da company değeri null geldiği için ayrıca şirket bilgileri alınıyor

            model.Company = company;
            var mailExtension = model.Company.Email.Substring(model.Company.Email.LastIndexOf('@') + 1);
            model.Email = EmailCreator.Create(model.Name, model.LastName, mailExtension); // Mail adresi oluşturmak için yazdığımız metot (Business/Concrete)
            //E-mail alanı boş geldiği için model validasyonu sağlamıyor ve email alanı yukarıda doldurulduğu halde ModelState.IsValid false olarak kalıyor. Bu yüzden ModelState temizlenerek yeniden doğrulama çalıştırılıyor.
            ModelState.Clear();
            TryValidateModel(model);

            var userByCitizenId = _userManager.Users.FirstOrDefault(x => x.CitizenId == model.CitizenId);

            if (userByCitizenId != null)
                ModelState.AddModelError("CitizenId", "T.C. Kimlik no başka bir kullanıcı tarafından kullanılıyor.");


            if (!ModelState.IsValid)
                return View(model);


            // profil fotoğrafı null check
            if (model.ProfilePhoto == null)
            {
                ModelState.AddModelError("ProfilePhoto", "Profil fotoğrafı boş bırakılamaz.");
                return View(model);
            }

            string photoUrl = await ImageSaver.SaveImage(model.ProfilePhoto);

            AppUser newUser = new AppUser()
            {
                CreateDate = DateTime.Now,
                CitizenId = model.CitizenId,
                Name = model.Name,
                SecondName = model.SecondName,
                LastName = model.LastName,
                SecondLastName = model.SecondLastName,
                Gender = model.Gender,
                Job = model.Job,
                Department = model.Department,
                BirthDate = model.BirthDate,
                StartDate = model.StartDate,
                Address = model.Address,
                PhoneNumber = model.PhoneNumber,
                PersonalPhone = model.PersonalPhone,
                ProfilePhoto = photoUrl,
                UserName = (model.Name + model.LastName).ToGlobal().ToLower(),
                Email = model.Email,
                Company = model.Company
            };
            CheckStatus(newUser);

            ValidationResult validationResult = await _managerValidator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return View();
            }

            //Username'in kayıtlı olup olmadığına bakar, kayıtlı ise sonuna sayı ekler
            newUser.UserName = newUser.UserName.Replace('ı', 'i');
            newUser.UserName = await CheckUserNameExistence(newUser.UserName);

            //Yeni oluşturulan yöneticiye gönderilecek mail ayarları
            string firstPassword = PasswordGenerator(); // Varsayılan şifre 
            var result = await _userManager.CreateAsync(newUser, firstPassword); // Identity ie yeni bir kullanıcı oluşturuluyor.
            if (!result.Succeeded)
                return View(model);

            await _userManager.AddToRoleAsync(newUser, "manager"); //Kullanıcıya rol ataması yapılıyor.

            if (result.Succeeded) // Kullanıcı oluşturma başarılı ise
            {
                // Email içeriği
                string content = "";

                //Çalışma ortamına göre adres belirleniyor
                if (_env.EnvironmentName == "Development")
                {
                    content =
                        $"<a href='https://localhost:44324" +
                        $"{Url.Action("Index", "Login", new { area = "" })}" +
                        $"'>Giriş Yap</a>" +
                        $"<br/>" +
                        $"Email: {newUser.Email}" +
                        $"<br/>" +
                        $"Şifreniz: {firstPassword}";
                    await _emailSender.SendEmailAsync("humaniksoftware@gmail.com", "Şifre Yenile", content); // Email gönderiliyor
                }
                else if (_env.EnvironmentName == "Production")
                {
                    content =
                        $"<a href='https://humanik.azurewebsites.net" +
                        $"{Url.Action("Index", "Login", new { area = "" })}" +
                        $"'>Giriş Yap</a>" +
                        $"<br/>" +
                        $"Email: {newUser.Email}" +
                        $"<br/>" +
                        $"Şifreniz: {firstPassword}";
                    await _emailSender.SendEmailAsync(newUser.Email, "Şifre Yenile", content); // Email gönderiliyor
                }

                TempData["Message"] = "Yönetici oluştruldu.";
                return RedirectToAction("Details", new { id = newUser.Id });
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Yönetici oluşturulamadı");
                return View(model);
            }
        }

        private async Task<string> CheckUserNameExistence(string username)
        {
            AppUser testUser = await _userManager.FindByNameAsync(username);
            if (testUser != null)
            {
                i++;
                username += i.ToString();
                await CheckUserNameExistence(username);
            }
            return username;
        }

        public async Task<IActionResult> List() // Yönetici listeleme ekranı
        {
            var userList = await _userManager.GetUsersInRoleAsync("manager");
            return View(userList);
        }

        public async Task<IActionResult> Details(int id) // Yönetici detayları ekranı
        {
            AppUser userDetails = await _userManager.FindByIdAsync(id.ToString());
            return View(userDetails);
        }


        [HttpGet]
        public async Task<IActionResult> Update(int id) // Yönetici güncelleme ekranı
        {
            List<Company> companies = _companyService.GetAll();
            ViewBag.Companies = companies;

            AppUser update = await _userManager.FindByIdAsync(id.ToString());
            ManagerViewModel vm = new ManagerViewModel();
            vm.Address = update.Address;
            vm.BirthDate = update.BirthDate;
            vm.CitizenId = update.CitizenId;
            vm.Name = update.Name;
            vm.LastName = update.LastName;
            vm.PersonalPhone = update.PersonalPhone;
            vm.PhoneNumber = update.PhoneNumber;
            vm.SecondName = update.SecondName;
            vm.SecondLastName = update.SecondLastName;
            vm.StartDate = update.StartDate;
            vm.Job = update.Job;
            vm.Gender = update.Gender;
            vm.Company = update.Company;
            vm.Department = update.Department;
            vm.ProfilPhotoPath = update.ProfilePhoto;
            vm.ID = update.Id;
            vm.Email = update.Email;
            vm.Status = update.Status;

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ManagerViewModel vm)
        {
            List<Company> companies = _companyService.GetAll();
            var test = ViewBag.Companies;
            ViewBag.Companies = companies;

            AppUser user = await _userManager.FindByIdAsync(vm.ID.ToString());
            var userByCitizenId = _userManager.Users.FirstOrDefault(x => x.CitizenId == vm.CitizenId && x.Id != user.Id);
            
            if (userByCitizenId != null)
                ModelState.AddModelError("CitizenId", "T.C. Kimlik no başka bir kullanıcı tarafından kullanılıyor.");

            if (!ModelState.IsValid)
                return View(vm);

            Company vmCompany = _companyService.GetById(vm.Company.CompanyID); // vm.Company'nin sadece Id'si geldiği için tüm bilgileri  ayrıca getiriliyor
            if (vm.Company.CompanyID != user.CompanyID || vm.Name != user.Name || vm.LastName != user.LastName)// isim, soyisim ya da şirket değişirse email yeniden oluşturulur
            {
                user.Email = EmailCreator.Create(vm.Name, vm.LastName, vmCompany.Name);
            }
            if (vm.Status != user.Status)
            {
                user.Status = vm.Status;
            }

            user.Address = vm.Address;
            user.BirthDate = vm.BirthDate;
            user.CitizenId = vm.CitizenId;
            user.Name = vm.Name;
            user.LastName = vm.LastName;
            user.PersonalPhone = vm.PersonalPhone;
            user.PhoneNumber = vm.PhoneNumber;
            user.SecondName = vm.SecondName;
            user.SecondLastName = vm.SecondLastName;
            user.StartDate = vm.StartDate;
            user.Job = vm.Job;
            user.Gender = vm.Gender;
            user.Company = vmCompany;
            user.Department = vm.Department;
            user.Id = vm.ID;
            //Eğer güncelleme sırasında yeni fotoğrag yüklenmediyse eski fotoğraf korunur, yüklendiyse URL güncellenir
            //Static Class olarak oluşturulan resim kaydetme metodu kullanılıyor (Business/Concrete)
            string photoUrl = vm.ProfilePhoto == null ? vm.ProfilPhotoPath : await ImageSaver.SaveImage(vm.ProfilePhoto);
            user.ProfilePhoto = photoUrl;

            ValidationResult validationResult = await _managerValidator.ValidateAsync(vm);
            if (!validationResult.IsValid)
            {
                return View();
            }
            await _userManager.UpdateAsync(user);

            TempData["Message"] = "Yönetici güncellendi.";
            return RedirectToAction("List", new { id = user.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id) // Yönetici sil (Pasife çek)
        {
            AppUser deleteUser = await _userManager.FindByIdAsync(id.ToString());
            deleteUser.Status = Status.Passive;
            deleteUser.DeleteDate = DateTime.Now;
            await _userManager.UpdateAsync(deleteUser);

            TempData["Message"] = "Yönetici silindi.";
            return RedirectToAction("List");
        }

        private string PasswordGenerator()
        {
            const string lowers = "abcdefghijklmnopqrstuvwxyz";
            const string uppers = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string numbers = "0123456789";
            const string puncs = ".+*!";

            StringBuilder res = new StringBuilder();
            Random rnd = new Random();

            for (int i = 0; i < 2; i++)
                res.Append(uppers[rnd.Next(uppers.Length)]);

            for (int i = 0; i < 6; i++)
                res.Append(lowers[rnd.Next(lowers.Length)]);

            for (int i = 0; i < 3; i++)
                res.Append(numbers[rnd.Next(numbers.Length)]);

            res.Append(puncs[rnd.Next(puncs.Length)]);

            return res.ToString();
        }

        private void CheckStatus(AppUser user)
        {
            if (user.StartDate <= DateTime.Now.Date)
                user.Status = Status.Active;
            else if (user.StartDate > DateTime.Now.Date)
                user.Status = Status.Pending;
        }
    }
}
