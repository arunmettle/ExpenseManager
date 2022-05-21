using ExpenseManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseManager.Interfaces
{
    public interface ISpendingLimitService
    {
        IEnumerable<SpendLimit> GetAllSpendCaps(string userName);
        void AddSpendCap(SpendLimit spendLimit, string userName);
        SpendLimit UpdateSpendCap(SpendLimit spendLimit,string userName);
        void DeleteSpendCap(string userName);
        SpendLimit GetSpendLimit(string userName);
    }
}
