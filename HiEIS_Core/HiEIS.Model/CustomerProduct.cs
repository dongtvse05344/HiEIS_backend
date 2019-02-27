using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiEIS.Model
{
    public class CustomerProduct
    {
        [Key]
        public Guid CustomerId { get; set; }
        [Key]
        public Guid ProductId { get; set; }
        public int Amount { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
    }
}
