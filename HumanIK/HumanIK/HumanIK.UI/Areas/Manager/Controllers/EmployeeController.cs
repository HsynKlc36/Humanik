using FluentValidation;
using FluentValidation.Results;
using HumanIK.BUSINESS.Abstract;
using HumanIK.BUSINESS.Concrete;
using HumanIK.ENTITIES.Entities;
using HumanIK.ENTITIES.Enums;
using HumanIK.UI.Areas.Manager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanIK.UI.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = "admin, manager")]
    public class EmployeeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IGenericService<Company> _companyService;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _env;
        private readonly IValidator<EmployeeViewModel> _validator;
        private int i = 1;

        public EmployeeController(UserManager<AppUser> userManager, IGenericService<Company> companyService, IEmailSender emailSender, IWebHostEnvironment env, IValidator<EmployeeViewModel> validator)
        {
            _userManager = userManager;
            _companyService = companyService;
            _emailSender = emailSender;
            _env = env;
            _validator = validator;

        }

        public async Task<IActionResult> Details(int id)
        {
            AppUser employee = await _userManager.FindByIdAsync(id.ToString());
            return View(employee);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(EmployeeViewModel vm)
        {
            AppUser user = await _userManager.GetUserAsync(User);
            Company company = _companyService.GetById(user.CompanyID);

            vm.Company = company;
            var mailExtension = vm.Company.Email.Substring(vm.Company.Email.LastIndexOf('@') + 1);
            vm.Email = EmailCreator.Create(vm.Name, vm.LastName, mailExtension); // Mail adresi oluşturmak için yazdığımız metot (Business/Concrete)
            //E-mail alanı boş geldiği için vm validasyonu sağlamıyor ve email alanı yukarıda doldurulduğu halde ModelState.IsValid false olarak kalıyor. Bu yüzden ModelState temizlenerek yeniden doğrulama çalıştırılıyor.

            ModelState.Clear();
            TryValidateModel(vm);

            var userByCitizenId = _userManager.Users.FirstOrDefault(x => x.CitizenId == vm.CitizenId);

            if (userByCitizenId != null)
                ModelState.AddModelError("CitizenId", "T.C. Kimlik no başka bir kullanıcı tarafından kullanılıyor.");

            if (vm.StartDate < company.Founded)
                ModelState.AddModelError("StartDate", $"İşe başlama tarihi şirketin kuruluş tarihi olan {company.Founded} tarihinden önce olamaz");

            if (!ModelState.IsValid)
                return View(vm);

            ValidationResult validationResult = await _validator.ValidateAsync(vm);
            if (!validationResult.IsValid)
            {
                return View();
            }

            // profil fotoğrafı null check
            if (vm.ProfilePhoto == null)
            {
                ModelState.AddModelError("ProfilePhotoFile", "Profil fotoğrafı boş bırakılamaz.");
                return View(vm);
            }

            string photoUrl = await ImageSaver.SaveImage(vm.ProfilePhoto);

            AppUser newUser = new AppUser()
            {
                CreateDate = DateTime.Now,
                CitizenId = vm.CitizenId,
                Name = vm.Name,
                SecondName = vm.SecondName,
                LastName = vm.LastName,
                SecondLastName = vm.SecondLastName,
                Gender = vm.Gender,
                Job = vm.Job,
                Department = vm.Department,
                BirthDate = vm.BirthDate,
                StartDate = vm.StartDate,
                Address = vm.Address,
                PhoneNumber = vm.PhoneNumber,
                PersonalPhone = vm.PersonalPhone,
                ProfilePhoto = photoUrl,
                UserName = (vm.Name + vm.LastName).ToGlobal().ToLower(),
                Email = vm.Email,
                Company = vm.Company,
                DateOfQuit = vm.DateOfQuit,
                AnnualRightOfVacation = CalculateVacationDay(vm.StartDate),
                Salary = vm.Salary,
                SalaryCurrencyUnit = vm.SalaryCurrencyUnit,
            };
            CheckStatus(newUser);

            //Username'in kayıtlı olup olmadığına bakar, kayıtlı ise sonuna sayı ekler
            newUser.UserName = newUser.UserName.Replace('ı', 'i');
            newUser.UserName = await CheckUserNameExistence(newUser.UserName);

            //Yeni oluşturulan yöneticiye gönderilecek mail ayarları
            string firstPassword = PasswordGenerator(); // Varsayılan şifre 
            var result = await _userManager.CreateAsync(newUser, firstPassword); // Identity ie yeni bir kullanıcı oluşturuluyor.
            await _userManager.AddToRoleAsync(newUser, "employee"); //Kullanıcıya rol ataması yapılıyor.

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
                //ViewBag.Message = "E-mail gönderildi.";


                TempData["Message"] = "Çalışan eklendi.";
                return RedirectToAction("Details", new { id = newUser.Id });
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Yönetici oluşturulamadı");
                return View(vm);
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

        [HttpGet]
        public async Task<IActionResult> Update(int id) // Yönetici güncelleme ekranı
        {

            List<Company> companies = _companyService.GetAll();
            ViewBag.Companies = companies;

            AppUser update = await _userManager.FindByIdAsync(id.ToString());
            EmployeeViewModel vm = new EmployeeViewModel();
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
            vm.Department = update.Department;
            vm.ProfilPhotoPath = update.ProfilePhoto;
            vm.ID = update.Id;
            vm.Email = update.Email;
            vm.Status = update.Status;
            vm.DateOfQuit = update.DateOfQuit != null ? (DateTime)update.DateOfQuit : null;
            vm.AnnualRightOfVacation = 14;
            vm.Salary = (int)update.Salary;
            vm.SalaryCurrencyUnit = (CurrencyUnit)update.SalaryCurrencyUnit;

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(EmployeeViewModel vm)
        {
            AppUser user = await _userManager.FindByIdAsync(vm.ID.ToString());
            Company company = _companyService.GetById(user.CompanyID);

            vm.Company = company;
            var mailExtension = vm.Company.Email.Substring(vm.Company.Email.LastIndexOf('@') + 1);
            vm.Email = EmailCreator.Create(vm.Name, vm.LastName, mailExtension); // Mail adresi oluşturmak için yazdığımız metot (Business/Concrete)
            //E-mail alanı boş geldiği için vm validasyonu sağlamıyor ve email alanı yukarıda doldurulduğu halde ModelState.IsValid false olarak kalıyor. Bu yüzden ModelState temizlenerek yeniden doğrulama çalıştırılıyor.

            if (vm.Status != user.Status)
            {
                user.Status = vm.Status;
            }

            var userByCitizenId = _userManager.Users.FirstOrDefault(x => x.CitizenId == vm.CitizenId && x.Id != user.Id);

            if (userByCitizenId != null)
                ModelState.AddModelError("CitizenId", "T.C. Kimlik no başka bir kullanıcı tarafından kullanılıyor.");

            ModelState.Clear();
            TryValidateModel(vm);

            if (vm.StartDate < company.Founded)
                ModelState.AddModelError("StartDate", $"İşe başlama tarihi şirketin kuruluş tarihi olan {company.Founded} tarihinden önce olamaz");

            if (!ModelState.IsValid)
                return View(vm);

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
            user.Company = vm.Company;
            user.Department = vm.Department;
            user.Id = vm.ID;
            user.Status = vm.Status;
            user.DateOfQuit = vm.DateOfQuit;
            user.AnnualRightOfVacation = CalculateVacationDay(user.StartDate);
            user.Salary = vm.Salary;
            user.Email = vm.Email;

            //Eğer güncelleme sırasında yeni fotoğrag yüklenmediyse eski fotoğraf korunur, yüklendiyse URL güncellenir
            //Static Class olarak oluşturulan resim kaydetme metodu kullanılıyor (Business/Concrete)
            string photoUrl = vm.ProfilePhoto == null ? vm.ProfilPhotoPath : await ImageSaver.SaveImage(vm.ProfilePhoto);
            user.ProfilePhoto = photoUrl;

            await _userManager.UpdateAsync(user);
            TempData["Message"] = "Çalışan güncellendi.";
            return RedirectToAction("List", new { id = user.Id });
        }

        public async Task<IActionResult> List() // Personel listeleme ekranı
        {
            var user = await _userManager.GetUserAsync(User);

            var userList = (IEnumerable<AppUser>)await _userManager.GetUsersInRoleAsync("employee");
            var employeesOfCompany = userList.Where(x => x.CompanyID == user.CompanyID);

            return View(employeesOfCompany);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id) // Yönetici sil (Pasife çek)
        {
            AppUser user = await _userManager.FindByIdAsync(id.ToString());
            user.Status = Status.Passive;
            user.DeleteDate = DateTime.Now;
            await _userManager.UpdateAsync(user);
            TempData["Message"] = "Çalışan silindi.";
            return RedirectToAction("List");
        }

        public int CalculateVacationDay(DateTime startDate)
        {
            double days = 365.2425;
            double diffDays = (DateTime.Now - startDate).TotalDays;

            double year = diffDays / days;

            if (1 <= year && year < 5)
            {
                return 14;
            }
            else if (5 <= year && year < 15)
            {
                return 20;
            }
            else if (15 <= year)
            {
                return 26;
            }
            else
            {
                return 0;
            }
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
