using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiEIS_Core.ViewModels
{
    public class CustomerVM
    {
        public Guid id { get; set; }
        public string Name { get; set; }
        public string Enterprise { get; set; }
        public string TaxNo { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }
        public string Fax { get; set; }
        public string Bank { get; set; }
        public string BankAccountNumber { get; set; }
        public string Email { get; set; }
    }

    public class CustomerCM
    {
        public string Enterprise { get; set; }
        public string TaxNo { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }
        public string Fax { get; set; }
        public string Bank { get; set; }
        public string BankAccountNumber { get; set; }
        public string Email { get; set; }

    }
    public class CustomerUM
    {
        public Guid id { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }
        public string Fax { get; set; }
        public string Bank { get; set; }
        public string BankAccountNumber { get; set; }
        public string Email { get; set; }

    }
}
