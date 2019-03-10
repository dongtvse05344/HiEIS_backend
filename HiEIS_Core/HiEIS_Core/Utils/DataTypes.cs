using HiEIS.Model;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HiEIS_Core.Utils
{
    public enum FileType
    {
        Template,
        Invoice 
    }
    public enum InvoiceType
    {
        New = 0,
        Approve =1,
        Signed = 3,
        Reject = 4
    }
    public enum UserRoles
    {
        [Display(Name = "Quản trị hệ thống")]
        Admin = 1,
        [Display(Name = "Quản trị viên")]
        Manager = 2,
        [Display(Name = "Kế toán trưởng")]
        AccountingManager = 3,
        [Display(Name = "Kế toán công nợ")]
        LiabilityAccountant = 4,
        [Display(Name = "Kế toán thanh toán")]
        PayableAccountant = 5,
        [Display(Name = "Khách hàng")]
        Customer = 6,
    }

    public static class RolesExtenstions
    {
        public static async Task InitAsync(RoleManager<IdentityRole> roleManager, UserManager<MyUser> userManager)
        {
            foreach (string roleName in Enum.GetNames(typeof(UserRoles)))
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            //await userManager.CreateAsync(new MyUser
            //{
            //    UserName = "admin",
            //    Email = "dong.tran@hisoft.vn"
            //}, "zaq@123");
        }
    }
}
