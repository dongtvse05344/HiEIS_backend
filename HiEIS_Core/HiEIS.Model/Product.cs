using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiEIS.Model
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Unit { get; set; }
        public float UnitPrice { get; set; }
        public float VATRate { get; set; }
        public Guid CompanyId { get; set; }
        public bool IsActive { get; set; }
        public bool HasIndex { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }

        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }
        //public virtual ICollection<CustomerProduct> CustomerProducts { get; set; }
        //public virtual ICollection<ProformaInvoiceItem> ProformaInvoiceItems { get; set; }
    }
}
