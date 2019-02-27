using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiEIS.Model
{
    public class Invoice : BaseEntity
    {
        public string LookupCode { get; set; }
        public string Number { get; set; }
        public int Type { get; set; }
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }
        public int PaymentMethod { get; set; }
        public string FileUrl { get; set; }
        public float SubTotal { get; set; }
        public float VATRate { get; set; }
        public float Total { get; set; }
        public string AmountInWords { get; set; }
        public int PaymentStatus { get; set; }
        public string Name { get; set; }
        public string Enterprise { get; set; }
        public string Address { get; set; }
        public string TaxNo { get; set; }
        public string Tel { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string BankAccountNumber { get; set; }
        public string Bank { get; set; }
        public string Note { get; set; }
        public Guid TemplateId { get; set; }
        public string StaffId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid CodeGuid { get; set; }

        [ForeignKey("StaffId")]
        public virtual Staff Staff { get; set; }
        [ForeignKey("TemplateId")]
        public virtual Template Template { get; set; }
        [ForeignKey("TemplateId")]
        public virtual Customer Customer { get; set; }

        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }
    }
}
