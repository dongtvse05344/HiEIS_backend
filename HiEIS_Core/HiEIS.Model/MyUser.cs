using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace HiEIS.Model
{
    public class MyUser : IdentityUser
    {
        public MyUser()
        {
            IsActive = true;
        }
        public String Name { get; set; }
        public bool IsActive { get; set; }

        public virtual Staff Staff { get; set; }
    }
}
