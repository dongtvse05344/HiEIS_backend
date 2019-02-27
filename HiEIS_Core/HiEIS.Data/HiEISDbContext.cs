using HiEIS.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HiEIS.Data
{
    public class HiEISDbContext : IdentityDbContext<MyUser>
    {
        public HiEISDbContext() : base((new DbContextOptionsBuilder())
            .UseLazyLoadingProxies()
            .UseSqlServer(@"Server=116.193.73.123;Database=HiEISDb_ver2;user id=sa;password=zaq@123;Trusted_Connection=True;Integrated Security=false;")
            .EnableSensitiveDataLogging(true)
            .Options)
        {

        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Template> Templates { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region Customer
            builder.Entity<Customer>().Property(_ => _.Name).IsUnicode();
            builder.Entity<Customer>().Property(_ => _.Enterprise).IsRequired().IsUnicode();
            builder.Entity<Customer>().Property(_ => _.TaxNo).HasMaxLength(18);
            builder.Entity<Customer>().Property(_ => _.Address).IsRequired();
            builder.Entity<Customer>().Property(_ => _.Tel).HasMaxLength(16).IsUnicode(false);
            builder.Entity<Customer>().Property(_ => _.Fax).HasMaxLength(16).IsUnicode(false);
            builder.Entity<Customer>().Property(_ => _.BankAccountNumber).HasMaxLength(50).IsUnicode(false);
            #endregion

            #region Company
            builder.Entity<Company>().Property(_ => _.TaxNo).HasMaxLength(18).IsRequired();
            builder.Entity<Company>().Property(_ => _.Name).IsUnicode().IsRequired();
            builder.Entity<Company>().Property(_ => _.Address).IsRequired();
            builder.Entity<Company>().Property(_ => _.Tel).HasMaxLength(20).IsUnicode(false);
            builder.Entity<Company>().Property(_ => _.Email).HasMaxLength(100);
            builder.Entity<Company>().Property(_ => _.Fax).HasMaxLength(50).IsUnicode(false);
            builder.Entity<Company>().Property(_ => _.BankAccountNumber).HasMaxLength(50).IsUnicode(false);
            builder.Entity<Company>().Property(_ => _.IsActive).HasDefaultValue(true);
            builder.Entity<Company>().Property(_ => _.CodeGuid).HasMaxLength(50);
            #endregion
            #region Staff
            builder.Entity<Staff>().Property(_ => _.Name).HasMaxLength(100);
            builder.Entity<Staff>().Property(_ => _.Name).HasMaxLength(100);

            builder.Entity<MyUser>()
                .HasOne(_ => _.Staff).WithOne(_ => _.MyUser)
                .HasForeignKey<Staff>(_ => _.Id);

            #endregion

        }

        public void Commit()
        {
            base.SaveChanges();
        }
    }
}
