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

        //public virtual ICollection<ProformaInvoice> ProformaInvoices { get; set; }
        //public virtual ICollection<CompanyCustomer> CompanyCustomers { get; set; }
        //public virtual ICollection<CustomerProduct> CustomerProducts { get; set; }
        //public virtual ICollection<Transaction> Transactions { get; set; }
        //public virtual ICollection<Invoice> Invoices { get; set; }
        //public virtual ICollection<User> Users { get; set; }
    }
}
