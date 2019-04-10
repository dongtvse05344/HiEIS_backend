using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiEIS_Core.ViewModels
{
    public class CompanyVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Enterprise { get; set; }
        public string TaxNo { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Fax { get; set; }
        public string Bank { get; set; }
        public string BankAccountNumber { get; set; }
        public bool IsActive { get; set; }
    }

    public class CompanyCM
    {
        public string Name { get; set; }
        public string TaxNo { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Fax { get; set; }
    }
    public class CompanyUM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string TaxNo { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Fax { get; set; }
    }

    public class EnterpriseTaxVM
    {
        public string MaSoThue { get; set; }
        public string Title { get; set; }
        public string DiaChiCongTy { get; set; }
        public bool IsDelete { get; set; }
    }
}
