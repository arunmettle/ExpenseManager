using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseManager.Areas.Identity.Data;
using ExpenseManager.Interfaces;
using ExpenseManager.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseManager.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly IExpenseService expenseService;
        private readonly ISpendingLimitService spendingLimitService;
        private readonly SignInManager<ApplicationrUser> signInManager;
        private UserManager<ApplicationrUser> userManager;
        private string userName;

        public ExpenseController(IExpenseService _expenseService, SignInManager<ApplicationrUser> _signInManager,
            UserManager<ApplicationrUser> _userManager, ISpendingLimitService _spendingLimitService)
        {
            expenseService = _expenseService;
            spendingLimitService = _spendingLimitService;
            signInManager = _signInManager;
            userManager = _userManager;
            userName = _signInManager.UserManager.GetUserName(_signInManager.Context.User);
        }
        public IActionResult Index(string searchString)
        {
            if (signInManager.IsSignedIn(User))
            {
                List<ExpenseReport> lstEmployee = new List<ExpenseReport>();
                lstEmployee = expenseService.GetAllExpenses(userName).ToList();

                if (!string.IsNullOrEmpty(searchString))
                {
                    lstEmployee = expenseService.GetSearchResult(searchString, userName).ToList();
                }
                return View(lstEmployee);
            }
            else
            {
                return Redirect("/Identity/Account/Login");
            }
            
        }

        public ActionResult AddEditExpenses(int itemId)
        {
            ExpenseReport model = new ExpenseReport();
            if (itemId > 0)
            {
                model = expenseService.GetExpenseData(itemId);
            }
            return PartialView("_expenseForm", model);
        }

        [HttpPost]
        public ActionResult Create(ExpenseReport newExpense)
        {
            if (ModelState.IsValid)
            {
                if (newExpense.ItemId > 0)
                {
                    expenseService.UpdateExpense(newExpense, userName);
                }
                else
                {
                    expenseService.AddExpense(newExpense, userName);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            expenseService.DeleteExpense(id);
            return RedirectToAction("Index");
        }

        public ActionResult ExpenseSummary()
        {
            return PartialView("_expenseReport");
        }

        public SpendLimit GetSpendingLimit(string userName)
        {
            return spendingLimitService.GetAllSpendCaps(userName).FirstOrDefault();
        }

        public JsonResult GetMonthlyExpense()
        {
            ExpenseReportData expenseReportData = new ExpenseReportData();
            SpendLimit spendLimit = GetSpendingLimit(userName);
            DateTime? dateTime = null;
            if(spendLimit.ItemId==0 && spendLimit.SpendingCap == 0)
            {
                dateTime = spendLimit.StartDate.AddDays(-28);
            }
            Dictionary<string, decimal> monthlyExpense = expenseService.CalculateMonthlyExpense(userName, dateTime);
            decimal sum = monthlyExpense.Sum(x => x.Value);

            expenseReportData = new ExpenseReportData
            {
                ExpenseValue = monthlyExpense,
                IsSpendExceeded = spendLimit.SpendingCap != 0 ? spendLimit.SpendingCap > sum : null,
                IsWeekly= spendLimit.IsWeekly
            };
            return new JsonResult(expenseReportData);
        }

        public JsonResult GetWeeklyExpense()
        {
            ExpenseReportData expenseReportData = new ExpenseReportData();
            SpendLimit spendLimit = GetSpendingLimit(userName);
            DateTime? dateTime = null;
            if (spendLimit.ItemId == 0 && spendLimit.SpendingCap == 0)
            {
                dateTime = spendLimit.StartDate.AddDays(-7);
            }
            Dictionary<string, decimal> weeklyExpense = expenseService.CalculateWeeklyExpense(userName, dateTime);
            decimal sum = weeklyExpense.Sum(x => x.Value);
            expenseReportData = new ExpenseReportData
            {
                ExpenseValue = weeklyExpense,
                IsSpendExceeded = spendLimit.SpendingCap != 0 ?  sum> spendLimit.SpendingCap : null, 
                IsWeekly= spendLimit.IsWeekly
            };
            return new JsonResult(expenseReportData);
        }
    }

    public class ExpenseReportData
    {
        public Dictionary<string, decimal> ExpenseValue { get; set; }
        public bool? IsSpendExceeded { get; set; }

        public bool? IsWeekly { get; set; }
    }
}