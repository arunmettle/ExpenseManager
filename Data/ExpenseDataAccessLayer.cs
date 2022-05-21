using ExpenseManager.Areas.Identity.Data;
using ExpenseManager.Interfaces;
using ExpenseManager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseManager.Data
{
    public class ExpenseDataAccessLayer : IExpenseService
    {
        private ExpenseManagerDbContext db;

        public ExpenseDataAccessLayer(ExpenseManagerDbContext _db)
        {
            db = _db;
        }
        public IEnumerable<ExpenseReport> GetAllExpenses(string userName)
        {
            try
            {
                return db.ExpenseReport.Where(x=>x.UserName==userName).ToList();
            }
            catch
            {
                throw;
            }
        }

        // To filter out the records based on the search string 
        public IEnumerable<ExpenseReport> GetSearchResult(string searchString, string userName)
        {
            List<ExpenseReport> exp = new List<ExpenseReport>();
            try
            {
                exp = GetAllExpenses(userName).ToList();
                return exp.Where(x => x.ItemName.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) != -1);
            }
            catch
            {
                throw;
            }
        }

        //To Add new Expense record       
        public void AddExpense(ExpenseReport expense, string userName)
        {
            try
            {
                expense.UserName = userName;
                db.ExpenseReport.Add(expense);
                db.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        //To Update the records of a particluar expense  
        public int UpdateExpense(ExpenseReport expense, string userName)
        {
            try
            {
                expense.UserName = userName;
                db.Entry(expense).State = EntityState.Modified;
                db.SaveChanges();

                return 1;
            }
            catch
            {
                throw;
            }
        }

        //Get the data for a particular expense  
        public ExpenseReport GetExpenseData(int id)
        {
            try
            {
                ExpenseReport expense = db.ExpenseReport.Find(id);
                return expense;
            }
            catch
            {
                throw;
            }
        }

        //To Delete the record of a particular expense  
        public void DeleteExpense(int id)
        {
            try
            {
                ExpenseReport emp = db.ExpenseReport.Find(id);
                db.ExpenseReport.Remove(emp);
                db.SaveChanges();

            }
            catch
            {
                throw;
            }
        }

        // To calculate last six months expense
        public Dictionary<string, decimal> CalculateMonthlyExpense(string userName, DateTime? monthTo= null)
        {
            List<ExpenseReport> lstEmployee = new List<ExpenseReport>();

            Dictionary<string, decimal> dictMonthlySum = new Dictionary<string, decimal>();

            decimal foodSum = db.ExpenseReport.Where
                //(cat => cat.Category == "Food" && (cat.ExpenseDate > DateTime.Now.AddMonths(-7)))
                (cat => cat.Category == "Food" && (cat.ExpenseDate >( monthTo==null? DateTime.Now.AddMonths(-7): monthTo.Value)))
                .Where(x=>x.UserName==userName)
                .Select(cat => cat.Amount)
                .Sum();

            decimal shoppingSum = db.ExpenseReport.Where
               //(cat => cat.Category == "Shopping" && (cat.ExpenseDate > DateTime.Now.AddMonths(-7)))
               (cat => cat.Category == "Shopping" && (cat.ExpenseDate > (monthTo == null ? DateTime.Now.AddMonths(-7) : monthTo.Value)))
               .Where(x => x.UserName == userName)
               .Select(cat => cat.Amount)
               .Sum();

            decimal travelSum = db.ExpenseReport.Where
               //(cat => cat.Category == "Travel" && (cat.ExpenseDate > DateTime.Now.AddMonths(-7)))
               (cat => cat.Category == "Travel" && (cat.ExpenseDate > (monthTo == null ? DateTime.Now.AddMonths(-7) : monthTo.Value)))
               .Where(x => x.UserName == userName)
               .Select(cat => cat.Amount)
               .Sum();

            decimal healthSum = db.ExpenseReport.Where
               //(cat => cat.Category == "Health" && (cat.ExpenseDate > DateTime.Now.AddMonths(-7)))
               (cat => cat.Category == "Health" && (cat.ExpenseDate > (monthTo == null ? DateTime.Now.AddMonths(-7) : monthTo.Value)))
               .Where(x => x.UserName == userName)
               .Select(cat => cat.Amount)
               .Sum();

            dictMonthlySum.Add("Food", foodSum);
            dictMonthlySum.Add("Shopping", shoppingSum);
            dictMonthlySum.Add("Travel", travelSum);
            dictMonthlySum.Add("Health", healthSum);

            return dictMonthlySum;
        }

        // To calculate last four weeks expense
        public Dictionary<string, decimal> CalculateWeeklyExpense(string userName, DateTime? weekTo=null)
        {
            List<ExpenseReport> lstEmployee = new List<ExpenseReport>();

            Dictionary<string, decimal> dictWeeklySum = new Dictionary<string, decimal>();

            decimal foodSum = db.ExpenseReport.Where
                //(cat => cat.Category == "Food" && (cat.ExpenseDate > DateTime.Now.AddDays(-28)))
                (cat => cat.Category == "Food" && (cat.ExpenseDate > DateTime.Now.AddDays(-28)))
                .Where(x => x.UserName == userName)
                .Select(cat => cat.Amount)
                .Sum();

            decimal shoppingSum = db.ExpenseReport.Where
               //(cat => cat.Category == "Shopping" && (cat.ExpenseDate > DateTime.Now.AddDays(-28)))
               (cat => cat.Category == "Shopping" && (cat.ExpenseDate > DateTime.Now.AddDays(-28)))
               .Where(x => x.UserName == userName)
               .Select(cat => cat.Amount)
               .Sum();

            decimal travelSum = db.ExpenseReport.Where
               //(cat => cat.Category == "Travel" && (cat.ExpenseDate > DateTime.Now.AddDays(-28)))
               (cat => cat.Category == "Travel" && (cat.ExpenseDate > DateTime.Now.AddDays(-28)))
               .Where(x => x.UserName == userName)
               .Select(cat => cat.Amount)
               .Sum();

            decimal healthSum = db.ExpenseReport.Where
               //(cat => cat.Category == "Health" && (cat.ExpenseDate > DateTime.Now.AddDays(-28)))
               (cat => cat.Category == "Health" && (cat.ExpenseDate > DateTime.Now.AddDays(-28)))
               .Where(x => x.UserName == userName)
               .Select(cat => cat.Amount)
               .Sum();

            dictWeeklySum.Add("Food", foodSum);
            dictWeeklySum.Add("Shopping", shoppingSum);
            dictWeeklySum.Add("Travel", travelSum);
            dictWeeklySum.Add("Health", healthSum);

            return dictWeeklySum;
        }

    }
}
