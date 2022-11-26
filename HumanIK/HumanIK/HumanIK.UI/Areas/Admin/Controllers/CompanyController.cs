using FluentValidation;
using FluentValidation.Results;
using FormHelper;
using HumanIK.BUSINESS.Abstract;
using HumanIK.BUSINESS.Concrete;
using HumanIK.ENTITIES.Entities;
using HumanIK.ENTITIES.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace HumanIK.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class CompanyController : Controller
    {
        private readonly IGenericService<Company> _companyService;
        private readonly IValidator<Company> _validator;
        private readonly IAppLevelVariables _appLevelVariables;
        private readonly UserManager<AppUser> _userManager;

        public CompanyController(IGenericService<Company> companyService, IValidator<Company> validator, IAppLevelVariables appLevelVariables, UserManager<AppUser> userManager)
        {
            _companyService = companyService;
            _validator = validator; //FluentValidation paketi ile oluşturulan validasyonlar
            _appLevelVariables = appLevelVariables; //Startup.cs'de Singleton olarak tanımlandığı için sorguyu yapan kim olursa olsun aynı instance döndürülür
            _userManager = userManager;
        }

        public IActionResult List() // Şirket bilgilerinin listelendiği ekran
        {
            CheckStartDealStatus();
            CheckAllDealStatus();
            return View(_companyService.GetAll());
        }
        public IActionResult Details(int id)
        {
            var company = _companyService.GetById(id);
            return View(company);
        }

        public IActionResult ActiveList()
        {
            var companyActive = _companyService.GetDefault(x => x.DealStatus == Status.Active);
            return View("List", companyActive);
        }
        public IActionResult PasiveList()
        {
            var companyPasive = _companyService.GetDefault(x => x.DealStatus == Status.Passive);
            return View("List", companyPasive);
        }
        public IActionResult PendingList()
        {
            var companyPending = _companyService.GetDefault(x => x.DealStatus == Status.Pending);
            return View("List", companyPending);
        }

        public IActionResult Search(string companyName) //Şirket adına göre arama sonuölarını döndüren action
        {
            if (companyName == null)
            {
                return View("List", _companyService.GetAll());
            }
            return View("List", _companyService.GetDefault(x => x.Name.Contains(companyName)));
        }

        public IActionResult Add() //Şirket ekleme ekranı
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Company company) //Şirket ekleme 
        {
            Company companybyMersisNo = _companyService.GetByDefault(x => x.MersisNo == company.MersisNo); //Girilen Mersis No'nun başka bir kullanıcı tarafından kullanılıp kullanılmadığı kontrol ediliyor
            if (companybyMersisNo != null)
                ModelState.AddModelError("MersisNo", "Mersis No başka bir şirket tarafından kullanılıyor.");

            Company companybyTaxNo = _companyService.GetByDefault(x => x.TaxNo == company.TaxNo);
            if (companybyTaxNo != null)
                ModelState.AddModelError("TaxNo", "Vergi No başka bir şirket tarafından kullanılıyor.");

            CompanyDeal(company);

            if (!ModelState.IsValid)
                return View(company);

            ValidationResult validationResult = await _validator.ValidateAsync(company); //Kendi oluşturduğumuz fluent validationlar ile kontrol sağlanıyor
            if (!validationResult.IsValid)
            {
                return View();
            }
            //Eğer güncelleme sırasında yeni fotoğrag yüklenmediyse eski fotoğraf korunur, yüklendiyse URL güncellenir
            //Static Class olarak oluşturulan resim kaydetme metodu kullanılıyor (Business/Concrete)
            string photoUrl = company.LogoFile == null ? company.Logo : await ImageSaver.SaveImage(company.LogoFile);
            company.Logo = photoUrl;
            CheckCompanyDealStatus(company);            
            _companyService.Add(company);

            TempData["Message"] = "Şirket eklendi.";
            return RedirectToAction("List");
        }

        public IActionResult Update(int id) //Şirket güncelleme ekranı
        {
            Company update = _companyService.GetById(id);
            return View(update);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Company company) //Şirket güncelleme
        {
            Company companybyMersisNo = _companyService.GetByDefault(x => x.MersisNo == company.MersisNo && x.CompanyID != company.CompanyID);
            if (companybyMersisNo != null)
                ModelState.AddModelError("MersisNo", "Mersis No başka bir şirket tarafından kullanılıyor.");

            Company companybyTaxNo = _companyService.GetByDefault(x => x.TaxNo == company.TaxNo && x.CompanyID != company.CompanyID);
            if (companybyTaxNo != null)
                ModelState.AddModelError("TaxNo", "Vergi No başka bir şirket tarafından kullanılıyor.");

            CompanyDeal(company);
            

            if (!ModelState.IsValid)
                return View(company);

            ValidationResult validationResult = await _validator.ValidateAsync(company);
            if (!validationResult.IsValid)
            {
                return View();
            }

            string photoUrl = company.LogoFile == null ? company.Logo : await ImageSaver.SaveImage(company.LogoFile);
            company.Logo = photoUrl;

            CheckCompanyDealStatus(company);

            _companyService.Update(company);
            TempData["Message"] = "Şirket güncellendi.";
            return RedirectToAction("List");
        }

        public IActionResult Delete(int id) //Şirket silme (Durumu pasife çeker, veritabanından silmez)
        {
            //Company company = _companyService.GetById(id);
            //List<AppUser> managers = (List<AppUser>)(await _userManager.GetUsersInRoleAsync("manager"));
            //List<AppUser> employees = (List<AppUser>)(await _userManager.GetUsersInRoleAsync("employee"));
            //managers.RemoveAll(x => x.CompanyID != id);
            //employees.RemoveAll(x => x.CompanyID != id);

            //DeactivateEmployees(managers); // Şirkete dahil yöneticiler şirket silindiği takdirde pasife çekilir
            //DeactivateEmployees(employees); // Şirkete dahil çalışanlar şirket silindiği takdirde pasife çekilir
            
            _companyService.Remove(id);
            TempData["Message"] = "Şirket silindi.";
            return RedirectToAction("List");
        }

        //IAppLevelVariables ve AppLevelVariables yapıları singleton olarak Startup.cs'ye eklendi.
        //Böylece metot içindeki kontrol ile beraber gün içinde kaç kullanıcı tarafından kaç kera çağırıldığından bağımsız olarak veritabanın günde en fazla bir defa güncellenmesi sağlandı. (Business katmanı)
        private void CheckAllDealStatus() //Tüm şirketlerin sözleşme bitiş tarihlerine göre durumları günde bir defa çalışacak şekilde günceller
        {
            if (DateTime.Now.Date > _appLevelVariables.LastCheckedDate) //_applevelVariables singleton olduğu için uygulama ömrü boyunca günde bir defa true olur.
            {
                _appLevelVariables.LastCheckedDate = DateTime.Now.Date; //Günde bir kere çalışması için LastCheckedDate güncelleniyor 
                List<Company> companies = _companyService.GetAll();
                if (companies != null)
                {
                    foreach (Company item in companies)
                    {
                        if (item.DealEndDate <= DateTime.Now.Date)
                        {
                            item.DealStatus = Status.Passive;
                            item.Status = Status.Passive;
                        }
                        else if(item.DealStartDate > DateTime.Now.Date)
                        {
                            item.DealStatus = Status.Pending;
                            item.Status = Status.Pending;
                        }
                        else
                        {
                            item.DealStatus = Status.Active;
                            item.Status = Status.Active;
                        }
                    }
                }
                _companyService.Update(companies);
            }
        }

        //Tek bir şirketin anlaşma durumunu kontrol eder
        private void CheckCompanyDealStatus(Company company)
        {
            if (company.DealEndDate <= DateTime.Now.Date)
            {
                company.DealStatus = Status.Passive;
                company.Status = Status.Passive;
            }
            else if (company.DealStartDate > DateTime.Now.Date)
            {
                company.DealStatus = Status.Pending;
                company.Status = Status.Pending;
            }
            else
            {
                company.DealStatus = Status.Active;
                company.Status = Status.Active;
            }
        }

        public void CompanyDeal(Company company)
        {
            DateTime dealDate = company.DealStartDate.AddDays(30);
            if (company.DealEndDate < dealDate)
            {
                ModelState.AddModelError("DealEndDate", "Anlaşma süresi en az 1 ay olmak zorundadır");
            }
        }

        // Şirket silinmesi durumunda ilişkili AppUser listesi pasife çekilir
        public void DeactivateEmployees(List<AppUser> list)
        {
            foreach (var person in list)
            {
                if (person.Status == Status.Active)
                {
                    person.Status = Status.Passive;
                }
            }
        }

        public void ActivateEmployees(List<AppUser> list)
        {
            foreach (var person in list)
            {
                if (person.Status == Status.Passive)
                {
                    person.Status = Status.Active;
                }
            }
        }

        public void CheckStartDealStatus()
        {
            if (DateTime.Now.Date > _appLevelVariables.LastCheckedDate)
            {
                _appLevelVariables.LastCheckedDate = DateTime.Now.Date;
                List<Company> companies = _companyService.GetAll();
                if (companies != null)
                {
                    foreach (Company item in companies)
                    {
                        if (item.DealStartDate > DateTime.Now.Date)
                        {
                            item.DealStatus = Status.Pending;
                        }
                    }
                }
            }
        }
    }
}
