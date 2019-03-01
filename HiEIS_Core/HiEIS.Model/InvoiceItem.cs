using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiEIS.Model
{
    public class InvoiceItem
    {
        [Key]
        public Guid InvoiceId { get; set; }
        [Key]
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }

        [ForeignKey("InvoiceId")]
        public virtual Invoice Invoice { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}
