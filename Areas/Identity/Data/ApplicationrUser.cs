using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseManager.Models;
using Microsoft.AspNetCore.Identity;

namespace ExpenseManager.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the ApplicationrUser class
    public class ApplicationrUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<ExpenseReport> ExpenseReports { get; set; }
        //public SpendLimit SpendLimit { get; set; }
    }
}
