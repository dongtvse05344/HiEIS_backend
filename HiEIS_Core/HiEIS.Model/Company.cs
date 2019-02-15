﻿using System;
using System.Collections.Generic;
using System.Text;

namespace HiEIS.Model
{
    public class Company : BaseEntity
    {
        public string TaxNo { get; set; }
        public string Name { get; set; }
        public string  Address { get; set; }
        public string Tel { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Fax { get; set; }
        public string Bank { get; set; }
        public string BankAccountNumber { get; set; }
        public bool IsActive { get; set; }
        public string CodeGuid { get; set; }

        public virtual ICollection<Staff> Staffs { get; set; }
    }
}
