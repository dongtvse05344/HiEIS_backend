﻿// <auto-generated />
using System;
using HiEIS.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HiEIS.Data.Migrations
{
    [DbContext(typeof(HiEISDbContext))]
    [Migration("20190314070943_dong14032019")]
    partial class dong14032019
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HiEIS.Model.Company", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .IsRequired();

                    b.Property<string>("Bank");

                    b.Property<string>("BankAccountNumber")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("CodeGuid")
                        .HasMaxLength(50);

                    b.Property<string>("Email")
                        .HasMaxLength(100);

                    b.Property<string>("Fax")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(true);

                    b.Property<string>("Name")
                        .IsRequired()
                        .IsUnicode(true);

                    b.Property<string>("TaxNo")
                        .IsRequired()
                        .HasMaxLength(18);

                    b.Property<string>("Tel")
                        .HasMaxLength(20)
                        .IsUnicode(false);

                    b.Property<string>("Website");

                    b.HasKey("Id");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("HiEIS.Model.CurrentSign", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<Guid>("CompanyId");

                    b.Property<DateTime>("DateExpiry");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("CurrentSigns");
                });

            modelBuilder.Entity("HiEIS.Model.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("Bank");

                    b.Property<string>("BankAccountNumber");

                    b.Property<string>("Enterprise");

                    b.Property<string>("Fax");

                    b.Property<string>("Name");

                    b.Property<string>("TaxNo");

                    b.Property<string>("Tel");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("HiEIS.Model.Invoice", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("AmountInWords");

                    b.Property<string>("Bank");

                    b.Property<string>("BankAccountNumber");

                    b.Property<DateTime>("Date");

                    b.Property<DateTime>("DueDate");

                    b.Property<string>("Enterprise");

                    b.Property<string>("Fax");

                    b.Property<string>("FileUrl");

                    b.Property<string>("Form");

                    b.Property<string>("LockupCode");

                    b.Property<string>("Name");

                    b.Property<int>("No")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Note");

                    b.Property<string>("Number");

                    b.Property<string>("PaymentMethod");

                    b.Property<int>("PaymentStatus");

                    b.Property<string>("Serial");

                    b.Property<string>("StaffId");

                    b.Property<float>("SubTotal");

                    b.Property<string>("TaxNo");

                    b.Property<string>("Tel");

                    b.Property<Guid>("TemplateId");

                    b.Property<float>("Total");

                    b.Property<int>("Type");

                    b.Property<float>("VATAmount");

                    b.Property<float>("VATRate");

                    b.HasKey("Id");

                    b.HasIndex("StaffId");

                    b.HasIndex("TemplateId");

                    b.ToTable("Invoices");
                });

            modelBuilder.Entity("HiEIS.Model.InvoiceItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("InvoiceId");

                    b.Property<string>("Name");

                    b.Property<Guid>("ProductId");

                    b.Property<int>("Quantity");

                    b.Property<Guid?>("TemplateId");

                    b.Property<float>("Total");

                    b.Property<string>("Unit");

                    b.Property<float>("UnitPrice");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("InvoiceItem");
                });

            modelBuilder.Entity("HiEIS.Model.MyUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("Name");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("HiEIS.Model.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<Guid>("CompanyId");

                    b.Property<bool>("IsActive");

                    b.Property<string>("Name");

                    b.Property<string>("Unit");

                    b.Property<float>("UnitPrice");

                    b.Property<float>("VATRate");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique()
                        .HasFilter("[Code] IS NOT NULL");

                    b.HasIndex("CompanyId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("HiEIS.Model.Staff", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("Code")
                        .IsUnicode(false);

                    b.Property<Guid>("CompanyId");

                    b.Property<string>("Name")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("Staffs");
                });

            modelBuilder.Entity("HiEIS.Model.Template", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("Amount");

                    b.Property<int>("BeginNo");

                    b.Property<Guid>("CompanyId");

                    b.Property<int>("CurrentNo");

                    b.Property<string>("FileUrl");

                    b.Property<string>("Form")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(true);

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(true);

                    b.Property<int>("Llx");

                    b.Property<int>("Lly");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(true);

                    b.Property<string>("ReleaseAnnouncementUrl");

                    b.Property<DateTime>("ReleaseDate");

                    b.Property<string>("Serial")
                        .IsRequired()
                        .IsUnicode(false);

                    b.Property<int>("Urx");

                    b.Property<int>("Ury");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("Templates");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("HiEIS.Model.CurrentSign", b =>
                {
                    b.HasOne("HiEIS.Model.Company", "Company")
                        .WithMany("CurrentSigns")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("HiEIS.Model.Invoice", b =>
                {
                    b.HasOne("HiEIS.Model.Staff", "Staff")
                        .WithMany("Invoices")
                        .HasForeignKey("StaffId");

                    b.HasOne("HiEIS.Model.Template", "Template")
                        .WithMany("Invoices")
                        .HasForeignKey("TemplateId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("HiEIS.Model.InvoiceItem", b =>
                {
                    b.HasOne("HiEIS.Model.Invoice", "Invoice")
                        .WithMany("InvoiceItems")
                        .HasForeignKey("InvoiceId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("HiEIS.Model.Product", "Product")
                        .WithMany("InvoiceItems")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("HiEIS.Model.Product", b =>
                {
                    b.HasOne("HiEIS.Model.Company", "Company")
                        .WithMany("Products")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("HiEIS.Model.Staff", b =>
                {
                    b.HasOne("HiEIS.Model.Company", "Company")
                        .WithMany("Staffs")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HiEIS.Model.MyUser", "MyUser")
                        .WithOne("Staff")
                        .HasForeignKey("HiEIS.Model.Staff", "Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("HiEIS.Model.Template", b =>
                {
                    b.HasOne("HiEIS.Model.Company", "Company")
                        .WithMany("Templates")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("HiEIS.Model.MyUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("HiEIS.Model.MyUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HiEIS.Model.MyUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("HiEIS.Model.MyUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
