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
    public class SpendingDataAccessLayer : ISpendingLimitService
    {
        private ExpenseManagerDbContext db;
        public SpendingDataAccessLayer(ExpenseManagerDbContext _db)
        {
            db = _db;
        }
        public void AddSpendCap(SpendLimit spendLimit, string userName)
        {
            try
            {
                List<SpendLimit> oldSpendLimits = db.SpendLimit.Where(x => x.UserName == userName).ToList();
                if (oldSpendLimits.Count > 0)
                {
                    foreach (SpendLimit item in oldSpendLimits)
                    {
                        db.SpendLimit.Remove(item);
                        db.SaveChanges();
                    }
                }
                spendLimit.UserName = userName;
                db.SpendLimit.Add(spendLimit);
                db.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public void DeleteSpendCap(string userName)
        {
            try
            {
                SpendLimit spl = db.SpendLimit.Where(x => x.UserName == userName).FirstOrDefault();
                db.SpendLimit.Remove(spl);
                db.SaveChanges();

            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<SpendLimit> GetAllSpendCaps(string userName)
        {
            try
            {
                return db.SpendLimit.Where(x => x.UserName == userName).ToList();
            }
            catch
            {
                throw;
            }
        }

        public SpendLimit UpdateSpendCap(SpendLimit spendLimit, string userName)
        {
            try
            {
                spendLimit.UserName = userName;
                db.Entry(spendLimit).State = EntityState.Modified;
                db.SaveChanges();
                return spendLimit;
            }
            catch
            {
                throw;
            }
        }

        public SpendLimit GetSpendLimit(string userName)
        {
            try
            {
                SpendLimit spl = db.SpendLimit.Where(x=>x.UserName == userName).FirstOrDefault();
                return spl;
            }
            catch (Exception)
            {

                throw;
            }
        }

        
    }
}
