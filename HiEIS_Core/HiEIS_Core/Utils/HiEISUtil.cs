using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiEIS_Core.Utils
{
    public static class HiEISUtil
    {
        public static List<SelectListItem> GetAllStaffRoles()
        {
            List<SelectListItem> roles = new List<SelectListItem>()
            {
                new SelectListItem{ Text = "Quản lý", Value = nameof(UserRoles.Manager) },
                new SelectListItem{ Text = "Kế toán trưởng", Value = nameof(UserRoles.AccountingManager) },
                new SelectListItem{ Text = "Kế toán công nợ", Value = nameof(UserRoles.LiabilityAccountant) },
                new SelectListItem{ Text = "Kế toán thanh toán", Value = nameof(UserRoles.PayableAccountant) },
            };

            return roles;
        }

        public static List<SelectListItem> GetStaffRoles()
        {
            List<SelectListItem> roles = new List<SelectListItem>()
            {
                new SelectListItem{ Text = "Kế toán trưởng", Value =  nameof(UserRoles.AccountingManager) },
                new SelectListItem{ Text = "Kế toán công nợ", Value =  nameof(UserRoles.LiabilityAccountant) },
                new SelectListItem{ Text = "Kế toán thanh toán", Value =  nameof(UserRoles.PayableAccountant) },
            };

            return roles;
        }
    }
}
