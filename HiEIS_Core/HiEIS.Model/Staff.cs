using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiEIS.Model
{
    public class Staff : BaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }
        public Guid CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }
        public virtual MyUser MyUser { get; set; }
        //public virtual User User { get; set; }

        public virtual ICollection<Invoice> Invoices { get; set; }
        public virtual ICollection<ProformaInvoice> ProformaInvoices { get; set; }
    }
}
