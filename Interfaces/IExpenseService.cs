using ExpenseManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseManager.Interfaces
{
    public interface IExpenseService
    {
        IEnumerable<ExpenseReport> GetAllExpenses(string userName);
        IEnumerable<ExpenseReport> GetSearchResult(string searchString, string userName);
        void AddExpense(ExpenseReport expense, string userName);
        int UpdateExpense(ExpenseReport expense, string userName);
        ExpenseReport GetExpenseData(int id);
        void DeleteExpense(int id);
        Dictionary<string, decimal> CalculateMonthlyExpense(string userName, DateTime? monthTo = null);
        Dictionary<string, decimal> CalculateWeeklyExpense(string userName, DateTime? weekTo = null);
    }
}
