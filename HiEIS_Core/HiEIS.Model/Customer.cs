using System;
using System.Collections.Generic;
using System.Text;

namespace HiEIS.Model
{
    public class Customer : BaseEntity
    {
        public string Name { get; set; }
        public string Enterprise { get; set; }
        public string TaxNo { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }
        public string Fax { get; set; }
        public string Bank { get; set; }
        public string BankAccountNumber { get; set; }
    }
}
