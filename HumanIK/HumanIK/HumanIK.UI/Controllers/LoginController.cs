using HumanIK.ENTITIES.Entities;
using HumanIK.ENTITIES.Enums;
using HumanIK.REPOSITORIES.Abstract;
using HumanIK.UI.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace HumanIK.UI.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _env;
        private readonly IGenericRepository<Company> _companyService;

        public LoginController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailSender emailSender, IWebHostEnvironment env, IGenericRepository<Company> companyService)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _env = env; // Çalışma ortamına ait bilgileri tutar
            _companyService = companyService;
            _signInManager = signInManager; // Identity üzerinden kullanıcı girişi, şifre yenileme ve değiştirme gibi işlemleri yapabilmemizi sağlar
        }

        [HttpGet]
        public IActionResult Index() // Giriş ekranı
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel vm) //Giriş işlemi
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(vm.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Hatalı şifre ya da kullanıcı adı!");
                    return View(vm);
                }

                if (!user.IsEverLoggedIn) // Sisteme yeni eklenen bir kullanıcı ise ilk girişinde şifresini değiştirmeye zorlanır
                {
                    NewPasswordViewModel npvm = new NewPasswordViewModel()
                    {
                        Email = vm.Email,
                    };
                    return View("ChangeFirstPassword", npvm); // email bilgisini tutan model ile view şifre değiştirme view'i çağırılır
                }

                Company company = _companyService.GetById(user.CompanyID);

                if (user.Status == Status.Active && company.Status == Status.Active)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, vm.Password, false, false); // Giriş yapma işlemi denenir
                     
                    if (result.Succeeded)
                    {
                        var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault(); // Giriş yapan kullanıcının rolü alınır
                        string companyName = _companyService.GetById(user.CompanyID).Name;
                        HttpContext.Session.SetString("CompanyName", companyName);

                        return RedirectToAction("index", $"{role}", new { area = $"{role}" }); // Rolüne göre uygun adrese yönlendirilir
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Hatalı şifre ya da kullanıcı adı!");
                        return View(vm);
                    }
                }
                else if(user.Status == Status.Pending)
                {
                    ModelState.AddModelError(string.Empty, "Hesabınız beklemededir, lütfen aktive olduktan sonra tekrar deneyin..");
                    return View(vm);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Giriş yetkiniz bulunmamaktadır. Lütfen şirket yöneticinizle iletişime geçin.");
                    return View(vm);
                }
            }

            return View(vm);
        }

        public IActionResult ForgotPassword() // Şifremi unuttum ekranı
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (email == null)
            {
                ViewBag.Message2 = "Bu alan boş geçilemez!";
                return View();
            }
            AppUser user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ViewBag.Message = "Lütfen geçerli bir e-mail giriniz.";
                return View();
            }
            else
            {
                string token = await _userManager.GeneratePasswordResetTokenAsync(user); // Token üretiliyor
                string encodedToken = HttpUtility.UrlEncode(token); // URL ile gönderileceği için bazı karakterlerin bozulmaması için encode ediliyor
                string generatedUrlExtension = Url.Action("NewPassword", "Login", new { email = email, token = encodedToken });

                ViewBag.GeneratedUrlExtension = generatedUrlExtension;


                //Mail içeriği
                string content = "";

                //Çalışma ortamına göre mailin içeriği belirleniyor
                if (_env.EnvironmentName == "Development")
                {
                    content = $"Merhaba {user.GetFullName()} , <br />" +
                        $"Şifreni yenilemek için linke tıklayabilirsin" +
                        $"<a href='https://localhost:44324{Url.Action("NewPassword", "Login", new { email = email, token = encodedToken })}'>Şifre Yenile</a> <br />" +
                        $"İyi çalışmalar dileriz.. <br /> <br />" +
                        $"HumanIK Software";
                }
                else if (_env.EnvironmentName == "Production")
                {
                    content = $"Merhaba {user.GetFullName()} , <br />" +
                        $"Şifreni yenilemek için linke tıklayabilirsin" +
                        $"<a href='https://humanik.azurewebsites.net{Url.Action("NewPassword", "Login", new { email = email, token = encodedToken })}'>Yenile</a>" +
                         $"İyi çalışmalar dileriz.. <br /> <br />" +
                        $"HumanIK Software";
                }
                await _emailSender.SendEmailAsync(email, "Şifre Yenile", content); //Mail gönderiliyor
                ViewBag.Message = "E-mail gönderildi.";

                return View();
            }

        }
        public IActionResult NewPassword(string email, string token) // Şifre sıfırlama ekranı
        {
            NewPasswordViewModel vm = new NewPasswordViewModel();
            vm.Email = email;
            vm.Token = token;
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> NewPassword(NewPasswordViewModel vm) // Şifre sıfırlama işlemi
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            AppUser user = await _userManager.FindByEmailAsync(vm.Email);
            string decodedtoken = HttpUtility.UrlDecode(vm.Token); // Encode edilen token decode ediliyor
            IdentityResult passwordChangeResult = await _userManager.ResetPasswordAsync(user, decodedtoken, vm.Password);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Logout() // Çıkış işlemi
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Login", new { area = "" });

        }

        public IActionResult ChangeFirstPassword() // Yen eklenen kulanıcının ilk şifresinin değiştirildiiği ekran
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangeFirstPassword(NewPasswordViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            AppUser user = await _userManager.FindByEmailAsync(vm.Email);

            IdentityResult changeResult = IdentityResult.Failed(); // Şifre değiştirme işleminin sonucu varsayılan olarak yanlış olarak atanıyor.
            var testSignInResult = await _signInManager.PasswordSignInAsync(user, vm.CurrentPassword, false, false); // Eski şifre alanına girilen değer ile giriş yapmaya çalışılıyor.
            //Eğer giriş denemesi başarılı ise eski şifre ve yeni şifrenin aynı olup olmadığına bakılıyor. Aynı ise hata veriliyor. Değilse şifre güncelleniyor.
            if (testSignInResult.Succeeded)
            {
                if (vm.CurrentPassword == vm.Password)
                {
                    ModelState.AddModelError("Password", "Yeni şifre eski şifre ile aynı olamaz.");
                    return View(vm);
                }
                else
                {
                    changeResult = await _userManager.ChangePasswordAsync(user, vm.CurrentPassword, vm.Password);
                }
                await _signInManager.SignOutAsync(); // Deneme için yapılan signin işlemi geri alınıyor.
            }
            else
            {
                ModelState.AddModelError("CurrentPassword", "Eski şifre hatalı.");
                return View(vm);
            }

            //şifre değiştirme işlemi başarılı ise kullanıcı girişi yapılıyor.
            if (changeResult.Succeeded)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(user, vm.Password, false, false); // Giriş yapılıyor

                if (signInResult.Succeeded)
                {
                    user.IsEverLoggedIn = true; // Giriş başarılı ise ilk giriş yapılıp yapılmadığı bilgisi güncelleniyor
                    await _userManager.UpdateAsync(user);
                    var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

                    return RedirectToAction("index", $"{role}", new { area = $"{role}" });
                }
            }

            return View(vm);
        }

        //public static string RenderPartialToString(Controller controller, string viewName, object model)
        //{
        //    controller.ViewData.Model = model;

        //    using (StringWriter sw = new StringWriter())
        //    {
        //        ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
        //        ViewContext viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
        //        viewResult.View.Render(viewContext, sw);

        //        return sw.GetStringBuilder().ToString();
        //    }
        //}
    }


}
