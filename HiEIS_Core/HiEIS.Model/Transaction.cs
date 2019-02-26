using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiEIS.Model
{
    public class Transaction : BaseEntity
    {
        public int Type { get; set; }
        public float Amount { get; set; }
        public DateTime Date { get; set; }
        public Guid CustomerId { get; set; }
        public Guid CompanyId { get; set; }
        public string Note { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }
    }
}
