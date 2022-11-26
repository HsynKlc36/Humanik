using FluentValidation;
using FluentValidation.Results;
using HumanIK.BUSINESS.Abstract;
using HumanIK.BUSINESS.Concrete;
using HumanIK.ENTITIES.Entities;
using HumanIK.ENTITIES.Enums;
using IronOcr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace HumanIK.UI.Areas.Employee.Controllers
{
    [Area("Employee")]
    [Authorize(Roles = "admin, employee")]
    public class ExpenseController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IGenericService<Expense> _expenseService;
        private readonly IValidator<Expense> _validator;

        public ExpenseController(UserManager<AppUser> userManager, IGenericService<Expense> expenseService, IValidator<Expense> validator)
        {
            _userManager = userManager;
            _expenseService = expenseService;
            _validator = validator;
        }
        public async Task<IActionResult> Add()
        {
            AppUser user = await _userManager.GetUserAsync(User);
            Expense expense = new Expense()
            {
                CurrencyUnit = (CurrencyUnit)user.SalaryCurrencyUnit
            };
            return View(expense);
        }
        

        [HttpPost]
        public async Task<IActionResult> Add(Expense model)
        {
            AppUser user = await _userManager.GetUserAsync(User);        

          
            Expense expense = new Expense()
            {
                DemandOwner = user,
                Amount = model.Amount,
                CurrencyUnit = (CurrencyUnit)user.SalaryCurrencyUnit,
                Description = model.Description,
                ExpenseType = model.ExpenseType,
                CreateDate = model.CreateDate,
                ExpenseFilePath = await ImageSaver.SaveImage(model.ExpenseFile),
                Status = Status.Pending
            };

            if (!ModelState.IsValid)
            {
                return View(expense);
            }

            ValidationResult validationResult = await _validator.ValidateAsync(model); //Kendi oluşturduğumuz fluent validationlar ile kontrol sağlanıyor
            if (!validationResult.IsValid)
            {
                return View();
            }

            _expenseService.Add(expense);

            TempData["Message"] = "Harcama talebi oluşturuldu.";
            return RedirectToAction("List");
        }

        public IActionResult Update(int id)
        {
            Expense expense = _expenseService.GetById(id);
            return View(expense);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Expense model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            ValidationResult validationResult = await _validator.ValidateAsync(model); //Kendi oluşturduğumuz fluent validationlar ile kontrol sağlanıyor
            if (!validationResult.IsValid)
            {
                return View();
            }

            _expenseService.Update(model);
            TempData["Message"] = "Harcama talebi güncellendi.";
            return RedirectToAction("List");
        }

        public async Task<IActionResult> List()
        {
            AppUser user = await _userManager.GetUserAsync(User);
            var expenses = _expenseService.GetDefault(x => x.ExpenseDemandOwnerId == user.Id);
            return View(expenses);
        }

        public async Task<IActionResult> Cancel(int id)
        {
            AppUser user = await _userManager.GetUserAsync(User);
            var advance = _expenseService.GetById(id);
            advance.ConfirmationStatus = ConfirmationStatus.No;
            _expenseService.Update(advance);
            TempData["Message"] = "Harcama Talebi İptal Edildi";
            return RedirectToAction("List");
        }
    }
}
