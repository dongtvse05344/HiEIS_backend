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
        
        public DbSet<Company> Companies { get; set; }
        //public DbSet<CompanyCustomer> CompanyCustomers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        //public DbSet<CustomerProduct> CustomerProducts { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        //public DbSet<InvoiceItem> InvoiceItems { get; set; }
        public DbSet<Product> Products { get; set; }
        //public DbSet<ProformaInvoice> ProformaInvoices { get; set; }
        //public DbSet<ProformaInvoiceItem> ProformaInvoiceItems { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Template> Templates { get; set; }
        //public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

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
            builder.Entity<Company>().HasMany(_ => _.Staffs).WithOne(_ => _.Company);
            builder.Entity<Company>().HasMany(_ => _.Products).WithOne(_ => _.Company);
            builder.Entity<Company>().HasMany(_ => _.Templates).WithOne(_ => _.Company);
            #endregion

            //#region CompanyCustomer
            //builder.Entity<CompanyCustomer>().HasKey(_ => new { _.CompanyId, _.CustomerId});
            //builder.Entity<CompanyCustomer>().Property(_ => _.Liabilities).HasColumnType("decimal");
            //builder.Entity<CompanyCustomer>()
            //    .HasOne(_ => _.Company).WithMany(_ => _.CompanyCustomers)
            //    .HasForeignKey(_ => _.CompanyId);
            //builder.Entity<CompanyCustomer>()
            //    .HasOne(_ => _.Customer).WithMany(_ => _.CompanyCustomers)
            //    .HasForeignKey(_ => _.CustomerId);
            //#endregion

            //#region Customer
            //builder.Entity<Customer>().Property(_ => _.Name).HasMaxLength(100).IsRequired().IsUnicode();
            //builder.Entity<Customer>().Property(_ => _.Enterprise).IsRequired().IsUnicode();
            //builder.Entity<Customer>().Property(_ => _.TaxNo).HasMaxLength(18);
            //builder.Entity<Customer>().Property(_ => _.Address).IsRequired();
            //builder.Entity<Customer>().Property(_ => _.Tel).HasMaxLength(16).IsUnicode(false);
            //builder.Entity<Customer>().Property(_ => _.Fax).HasMaxLength(16).IsUnicode(false);
            //builder.Entity<Customer>().Property(_ => _.BankAccountNumber).HasMaxLength(50).IsUnicode(false);
            //builder.Entity<Customer>().HasMany(_ => _.ProformaInvoices).WithOne(_ => _.Customer);
            //builder.Entity<Customer>().HasMany(_ => _.CompanyCustomers).WithOne(_ => _.Customer);
            //builder.Entity<Customer>().HasMany(_ => _.CustomerProducts).WithOne(_ => _.Customer);
            //builder.Entity<Customer>().HasMany(_ => _.Transactions).WithOne(_ => _.Customer);
            //builder.Entity<Customer>().HasMany(_ => _.Invoices).WithOne(_ => _.Customer);
            //#endregion

            //#region CustomerProduct
            //builder.Entity<CustomerProduct>().HasKey(_ => new { _.CustomerId, _.ProductId});
            //builder.Entity<CustomerProduct>().Property(_ => _.Amount).IsRequired();
            //builder.Entity<CustomerProduct>()
            //    .HasOne(_ => _.Customer).WithMany(_ => _.CustomerProducts)
            //    .HasForeignKey(_ => _.CustomerId);
            //builder.Entity<CustomerProduct>()
            //    .HasOne(_ => _.Product).WithMany(_ => _.CustomerProducts)
            //    .HasForeignKey(_ => _.ProductId);
            //#endregion

            #region Invoice
            //builder.Entity<Invoice>().Property(_ => _.LookupCode).HasMaxLength(50).IsRequired().IsUnicode(false);
            //builder.Entity<Invoice>().Property(_ => _.Number).HasMaxLength(50).IsRequired().IsUnicode(false);
            //builder.Entity<Invoice>().Property(_ => _.Type).IsRequired();
            //builder.Entity<Invoice>().Property(_ => _.Date).IsRequired();
            //builder.Entity<Invoice>().Property(_ => _.PaymentMethod).IsRequired();
            //builder.Entity<Invoice>().Property(_ => _.SubTotal).HasColumnType("decimal");
            //builder.Entity<Invoice>().Property(_ => _.VATRate).HasColumnType("decimal");
            //builder.Entity<Invoice>().Property(_ => _.Total).HasColumnType("decimal");
            //builder.Entity<Invoice>().Property(_ => _.AmountInWords).IsRequired();
            //builder.Entity<Invoice>().Property(_ => _.Type).IsRequired().IsUnicode();
            //builder.Entity<Invoice>().Property(_ => _.Name).HasMaxLength(100).IsUnicode();
            //builder.Entity<Invoice>().Property(_ => _.Enterprise).IsRequired().IsUnicode();
            //builder.Entity<Invoice>().Property(_ => _.Address).IsRequired().IsUnicode();
            //builder.Entity<Invoice>().Property(_ => _.TaxNo).HasMaxLength(18).IsRequired();
            //builder.Entity<Invoice>().Property(_ => _.Tel).HasMaxLength(16).IsUnicode(false);
            //builder.Entity<Invoice>().Property(_ => _.Fax).HasMaxLength(16).IsUnicode(false);
            //builder.Entity<Invoice>().Property(_ => _.Email).IsUnicode();
            //builder.Entity<Invoice>().Property(_ => _.BankAccountNumber).HasMaxLength(50).IsUnicode(false);
            //builder.Entity<Invoice>().Property(_ => _.TemplateId).IsRequired();
            //builder.Entity<Invoice>().Property(_ => _.StaffId).IsRequired();
            //builder.Entity<Invoice>().Property(_ => _.CustomerId).IsRequired();
            //builder.Entity<Invoice>().Property(_ => _.CodeGuid).IsRequired();
            builder.Entity<Invoice>()
                .HasOne(_ => _.Staff).WithMany(_ => _.Invoices)
                .HasForeignKey(_ => _.StaffId);
            builder.Entity<Invoice>()
                .HasOne(_ => _.Template).WithMany(_ => _.Invoices)
                .HasForeignKey(_ => _.TemplateId);
            //builder.Entity<Invoice>()
            //    .HasOne(_ => _.Customer).WithMany(_ => _.Invoices)
            //    .HasForeignKey(_ => _.CustomerId);
            //builder.Entity<Invoice>().HasMany(_ => _.InvoiceItems).WithOne(_ => _.Invoice);
            #endregion

            #region InvoiceItem
            //builder.Entity<InvoiceItem>().HasKey(_ => new { _.InvoiceId, _.ProductId });
            //builder.Entity<InvoiceItem>().Property(_ => _.Quantity).IsRequired();
            //builder.Entity<InvoiceItem>().Property(_ => _.UnitPrice).IsRequired().HasColumnType("decimal");
            //builder.Entity<InvoiceItem>().Property(_ => _.VATRate).HasColumnType("decimal");
            builder.Entity<InvoiceItem>()
                .HasOne(_ => _.Invoice).WithMany(_ => _.InvoiceItems)
                .HasForeignKey(_ => _.InvoiceId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<InvoiceItem>()
                .HasOne(_ => _.Product).WithMany(_ => _.InvoiceItems)
                .HasForeignKey(_ => _.ProductId).OnDelete(DeleteBehavior.Restrict); ;
            #endregion

            #region MyUser
            //builder.Entity<MyUser>().Property(_ => _.Name).HasMaxLength(100).IsUnicode();
            //builder.Entity<MyUser>().Property(_ => _.IsActive).HasDefaultValue(true);
            builder.Entity<MyUser>()
                .HasOne(_ => _.Staff).WithOne(_ => _.MyUser)
                .HasForeignKey<Staff>(_ => _.Id);
            #endregion

            #region Product
            //builder.Entity<Product>().Property(_ => _.Name).IsRequired().IsUnicode();
            builder.Entity<Product>().HasIndex(_ => _.Code).IsUnique();
            
            //builder.Entity<Product>().Property(_ => _.Unit).IsRequired().IsUnicode();
            //builder.Entity<Product>().Property(_ => _.UnitPrice).IsRequired().HasColumnType("decimal");
            //builder.Entity<Product>().Property(_ => _.VATRate).IsRequired().HasColumnType("decimal");
            //builder.Entity<Product>().Property(_ => _.CompanyId).IsRequired();
            //builder.Entity<Product>().Property(_ => _.IsActive).IsRequired().HasDefaultValue(true);
            //builder.Entity<Product>().Property(_ => _.HasIndex).HasDefaultValue(false);
            //builder.Entity<Product>()
            //    .HasOne(_ => _.Company).WithMany(_ => _.Products)
            //    .HasForeignKey(_ => _.CompanyId);
            //builder.Entity<Product>().HasMany(_ => _.InvoiceItems).WithOne(_ => _.Product);
            //builder.Entity<Product>().HasMany(_ => _.CustomerProducts).WithOne(_ => _.Product);
            //builder.Entity<Product>().HasMany(_ => _.ProformaInvoiceItems).WithOne(_ => _.Product);
            #endregion

            //#region ProformaInvoice
            //builder.Entity<ProformaInvoice>().Property(_ => _.LookupCode).HasMaxLength(50).IsRequired().IsUnicode(false);
            //builder.Entity<ProformaInvoice>().Property(_ => _.Date).IsRequired();
            //builder.Entity<ProformaInvoice>().Property(_ => _.Status).IsRequired();
            //builder.Entity<ProformaInvoice>().Property(_ => _.SubTotal).IsRequired().HasColumnType("decimal");
            //builder.Entity<ProformaInvoice>().Property(_ => _.VATAmount).HasColumnType("decimal");
            //builder.Entity<ProformaInvoice>().Property(_ => _.TotalNoLiabilities).HasColumnType("decimal");
            //builder.Entity<ProformaInvoice>().Property(_ => _.Total).IsRequired().HasColumnType("decimal");
            //builder.Entity<ProformaInvoice>().Property(_ => _.StaffId).IsRequired();
            //builder.Entity<ProformaInvoice>().Property(_ => _.CustomerId).IsRequired();
            //builder.Entity<ProformaInvoice>().Property(_ => _.Liabilities).HasColumnType("decimal");
            //builder.Entity<ProformaInvoice>()
            //    .HasOne(_ => _.Staff).WithMany(_ => _.ProformaInvoices)
            //    .HasForeignKey(_ => _.StaffId);
            //builder.Entity<ProformaInvoice>()
            //    .HasOne(_ => _.Customer).WithMany(_ => _.ProformaInvoices)
            //    .HasForeignKey(_ => _.CustomerId);
            //builder.Entity<ProformaInvoice>().HasMany(_ => _.ProformaInvoiceItems).WithOne(_ => _.ProformaInvoice);
            //#endregion

            //#region ProformaInvoiceItem
            //builder.Entity<ProformaInvoiceItem>().HasKey(_ => new { _.ProformaInvoiceId, _.ProductId });
            //builder.Entity<ProformaInvoiceItem>().Property(_ => _.VATRate).HasColumnType("decimal");
            //builder.Entity<ProformaInvoiceItem>().Property(_ => _.Quantity).IsRequired();
            //builder.Entity<ProformaInvoiceItem>().Property(_ => _.UnitPrice).IsRequired().HasColumnType("decimal");
            //builder.Entity<ProformaInvoiceItem>()
            //    .HasOne(_ => _.ProformaInvoice).WithMany(_ => _.ProformaInvoiceItems)
            //    .HasForeignKey(_ => _.ProformaInvoiceId);
            //builder.Entity<ProformaInvoiceItem>()
            //    .HasOne(_ => _.Product).WithMany(_ => _.ProformaInvoiceItems)
            //    .HasForeignKey(_ => _.ProductId);
            //#endregion

            #region Staff
            builder.Entity<Staff>().Property(_ => _.Name).HasMaxLength(100);
            builder.Entity<Staff>().Property(_ => _.Code).IsUnicode(false);
            builder.Entity<Staff>().Property(_ => _.CompanyId).IsRequired();
            builder.Entity<MyUser>()
                .HasOne(_ => _.Staff).WithOne(_ => _.MyUser)
                .HasForeignKey<Staff>(_ => _.Id);

            //builder.Entity<Staff>().Property(_ => _.Id).IsRequired();
            //builder.Entity<Staff>().Property(_ => _.Name).HasMaxLength(100).IsUnicode();
            //builder.Entity<Staff>().Property(_ => _.Code).IsUnicode(false);
            //builder.Entity<Staff>().Property(_ => _.CompanyId).IsRequired();
            //builder.Entity<Staff>()
            //    .HasOne(_ => _.Company).WithMany(_ => _.Staffs)
            //    .HasForeignKey(_ => _.CompanyId);
            //builder.Entity<Staff>()
            //    .HasOne(_ => _.MyUser).WithOne(_ => _.Staff);
            //builder.Entity<Staff>().HasMany(_ => _.Invoices).WithOne(_ => _.Staff);
            //builder.Entity<Staff>().HasMany(_ => _.ProformaInvoices).WithOne(_ => _.Staff);
            #endregion

            #region Template
            builder.Entity<Template>().Property(_ => _.Name).HasMaxLength(100).IsRequired().IsUnicode();
            builder.Entity<Template>().Property(_ => _.Form).HasMaxLength(100).IsRequired().IsUnicode();
            builder.Entity<Template>().Property(_ => _.Serial).IsRequired().IsUnicode(false);
            builder.Entity<Template>().Property(_ => _.CompanyId).IsRequired();
            builder.Entity<Template>().Property(_ => _.IsActive).HasDefaultValue(true);
            //builder.Entity<Template>()
            //    .HasOne(_ => _.Company).WithMany(_ => _.Templates)
            //    .HasForeignKey(_ => _.CompanyId);
            //builder.Entity<Template>().HasMany(_ => _.Invoices).WithOne(_ => _.Template);
            #endregion

            //#region Transaction
            //builder.Entity<Transaction>().Property(_ => _.Type).IsRequired();
            //builder.Entity<Transaction>().Property(_ => _.Amount).HasColumnType("decimal");
            //builder.Entity<Transaction>().Property(_ => _.Date).IsRequired();
            //builder.Entity<Transaction>().Property(_ => _.CustomerId).IsRequired();
            //builder.Entity<Transaction>().Property(_ => _.CompanyId).IsRequired();
            //builder.Entity<Transaction>().Property(_ => _.Note).IsUnicode();
            //builder.Entity<Transaction>()
            //    .HasOne(_ => _.Customer).WithMany(_ => _.Transactions)
            //    .HasForeignKey(_ => _.CustomerId);
            //builder.Entity<Transaction>()
            //    .HasOne(_ => _.Company).WithMany(_ => _.Transactions)
            //    .HasForeignKey(_ => _.CompanyId);
            //#endregion

        }

        public void Commit()
        {
            base.SaveChanges();
        }
    }
}
