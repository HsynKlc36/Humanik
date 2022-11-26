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
    public class ExpenseController : Controller
    {
        private readonly IGenericService<Expense> _expenseService;
        private readonly UserManager<AppUser> _userManager;

        public ExpenseController(IGenericService<Expense> expenseService, UserManager<AppUser> userManager)
        {
            _expenseService = expenseService;
            _userManager = userManager;
        }
        public IActionResult Add()
        {
            return View();
        }

        public async Task<IActionResult> List()
        {
            AppUser user = await _userManager.GetUserAsync(User);
            var users = _userManager.Users;
            var all = _expenseService.GetAll();
            var expenses = _expenseService.GetDefault(x => x.DemandOwner.CompanyID == user.CompanyID)?.OrderByDescending(x => x.ConfirmationStatus).ThenBy(x => x.CreateDate);
            // js ile sayfayı yeniliyoruz çünkü yönlendirdiğimiz action'ların view ları yok bu sayfada açılıyor hepsi(AdvanceRefuseRequest,AdvanceConfirmRequest)


            //Admin olarak sorgulandığında dolu gelen DemandOwner property'leri manager olarak sorgulandığında boş geliyor. Bu yüzden manuel olarak dolduruyoruz.

            foreach (var expense in expenses)
            {
                var demandOwner = users.FirstOrDefault(x => x.Id == expense.ExpenseDemandOwnerId);
                expense.DemandOwner = demandOwner;
            }

            return View(expenses);
        }

        public IActionResult Refuse(int id, string demandOwner)//talep reddetme
        {
            var expense = _expenseService.GetById(id);
            ViewBag.DemandOwner = demandOwner;
            return View(expense);
        }

        public IActionResult ConfirmRefusing(int id)
        {
            var expense = _expenseService.GetById(id);
            expense.ConfirmationStatus = ConfirmationStatus.No;
            expense.DateOfReply = DateTime.Now;
            _expenseService.Update(expense);
            TempData["Message"] = "Harcama talebi reddedildi.";
            return RedirectToAction("List");
        }

        public IActionResult Confirm(int id)//talep onaylama 
        {
            var expense = _expenseService.GetById(id);
            expense.ConfirmationStatus = ConfirmationStatus.Yes;
            expense.DateOfReply = DateTime.Now;
            _expenseService.Update(expense);
            TempData["Message"] = "Harcama talebi onaylandı.";
            return RedirectToAction("List");
        }
    }
}
