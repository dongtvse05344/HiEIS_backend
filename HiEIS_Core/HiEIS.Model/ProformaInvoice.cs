using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiEIS.Model
{
    public class ProformaInvoice : BaseEntity
    {
        public string LookupCode { get; set; }
        public DateTime Date { get; set; }
        public string FileUrl { get; set; }
        public DateTime PaymentDeadline { get; set; }
        public int Status { get; set; }
        public float SubTotal { get; set; }
        public float VATAmount { get; set; }
        public float TotalNoLiabilities { get; set; }
        public float Total { get; set; }
        public Guid StaffId { get; set; }
        public Guid CustomerId { get; set; }
        public float Liabilities { get; set; }

        [ForeignKey("StaffId")]
        public virtual Staff Staff { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        public virtual ICollection<ProformaInvoiceItem> ProformaInvoiceItems { get; set; }
    }
}
