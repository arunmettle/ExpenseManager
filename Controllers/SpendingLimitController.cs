using ExpenseManager.Areas.Identity.Data;
using ExpenseManager.Interfaces;
using ExpenseManager.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseManager.Controllers
{
    public class SpendingLimitController : Controller
    {
        private readonly ISpendingLimitService spendingLimitService;
        private readonly SignInManager<ApplicationrUser> signInManager;
        private UserManager<ApplicationrUser> userManager;
        private string userName;

        public SpendingLimitController(ISpendingLimitService _spendingLimitService, SignInManager<ApplicationrUser> _signInManager, UserManager<ApplicationrUser> _userManager)
        {
            spendingLimitService = _spendingLimitService;
            signInManager = _signInManager;
            userManager = _userManager;
            userName = _signInManager.UserManager.GetUserName(_signInManager.Context.User);
        }
        public IActionResult Index()
        {

            if (signInManager.IsSignedIn(User))
            {
                List<SpendLimit> spendLimits = new List<SpendLimit>();
                spendLimits = spendingLimitService.GetAllSpendCaps(userName).ToList();
                if (spendLimits.Count <= 0)
                {
                    SpendLimit spendLimit = new SpendLimit
                    {
                        UserName = userName
                    };
                    spendLimits.Add(spendLimit);
                }
                return View(spendLimits);
            }
            else
            {
                return Redirect("/Identity/Account/Login");
            }
        }

        public ActionResult AddEditSpend(string userName)
        {
            SpendLimit model = new SpendLimit();
            if (!string.IsNullOrEmpty(userName))
            {
                model = spendingLimitService.GetSpendLimit(userName);
                if (model == null)
                {
                    model = new SpendLimit
                    {
                        UserName = userName
                    };
                }
            }
            return PartialView("_spendLimitForm", model);
        }

        [HttpPost]
        public ActionResult Create(SpendLimit spendLimit)
        {
            if (ModelState.IsValid)
            {
                if (spendLimit.ItemId > 0)
                {
                    spendingLimitService.UpdateSpendCap(spendLimit, userName);
                }
                else
                {
                    spendingLimitService.AddSpendCap(spendLimit, userName);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(string userName)
        {
            spendingLimitService.DeleteSpendCap(userName);
            return RedirectToAction("Index");
        }
    }
}
