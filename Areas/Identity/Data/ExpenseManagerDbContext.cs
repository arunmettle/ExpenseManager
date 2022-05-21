using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseManager.Areas.Identity.Data;
using ExpenseManager.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;

namespace ExpenseManager.Areas.Identity.Data
{
    public class ExpenseManagerDbContext : IdentityDbContext<ApplicationrUser>
    {
        private IConfiguration configuration;
        public ExpenseManagerDbContext(DbContextOptions<ExpenseManagerDbContext> options, IConfiguration iConfig)
            : base(options)
        {
            configuration = iConfig;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());

        }
        public virtual DbSet<ApplicationrUser> ApplicationrUsers { get; set; }
        public virtual DbSet<ExpenseReport> ExpenseReport { get; set; }
        public virtual DbSet<SpendLimit> SpendLimit { get; set; }
    }

    public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationrUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationrUser> builder)
        {

            builder.Property(x => x.FirstName).HasMaxLength(255);
            builder.Property(x => x.LastName).HasMaxLength(255);
        }
    }
}
