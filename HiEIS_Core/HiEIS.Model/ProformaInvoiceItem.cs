using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiEIS.Model
{
    public class ProformaInvoiceItem
    {
        public Guid ProformaInvoiceId { get; set; }
        public Guid ProductId { get; set; }
        public float VATRate { get; set; }
        public int Quantity { get; set; }
        public float UnitPrice { get; set; }
        public int OldNumber { get; set; }
        public int NewNumber { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        [ForeignKey("ProformaInvoiceId")]
        public virtual ICollection<ProformaInvoice> ProformaInvoices { get; set; }
        [ForeignKey("ProductId")]
        public virtual ICollection<Product> Products { get; set; }
    }
}
