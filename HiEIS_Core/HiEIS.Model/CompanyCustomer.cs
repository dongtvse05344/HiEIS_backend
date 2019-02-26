using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiEIS.Model
{
    public class CompanyCustomer
    {
        public Guid CustomerId { get; set; }
        public Guid CompanyId { get; set; }
        public float Liabilities { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
    }
}
