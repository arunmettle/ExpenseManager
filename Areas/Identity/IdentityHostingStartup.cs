using System;
using ExpenseManager.Areas.Identity.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(ExpenseManager.Areas.Identity.IdentityHostingStartup))]
namespace ExpenseManager.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<ExpenseManagerDbContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("ExpenseManagerDbContextConnection")));

                services.AddDefaultIdentity<ApplicationrUser>(options => options.SignIn.RequireConfirmedAccount = false)
                    .AddEntityFrameworkStores<ExpenseManagerDbContext>();
            });
        }
    }
}