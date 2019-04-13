using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiEIS.Model
{
    public class Invoice : BaseEntity
    {
        public Invoice(){
            DateCreated = DateTime.Now;
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int No { get; set; }
        public string LockupCode { get; set; }
        //Mẫu số
        public string Form { get; set; }
        //Kí hiệu
        public string Serial { get; set; }
        public string Number { get; set; }
        //Loại HĐ (GTGT / Bán hàng)
        public int Type { get; set; }
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }
        public string PaymentMethod { get; set; }
        public int PaymentStatus { get; set; }
        public string FileUrl { get; set; }

        //Thông tin khách hàng
        public string Name { get; set; }
        public string Enterprise { get; set; }
        public string TaxNo { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }
        public string Fax { get; set; }
        public string Bank { get; set; }
        public string BankAccountNumber { get; set; }
        
        //Cộng tiền hàng (chưa tính thuế)
        public float SubTotal { get; set; }
        public float VATRate { get; set; }
        public float VATAmount { get; set; }
        public float Total { get; set; }
        public string AmountInWords { get; set; }
        public DateTime DateCreated { get; set; }
        public string Note { get; set; }
        public Guid TemplateId { get; set; }
        public string StaffId { get; set; }
        //   public Guid CustomerId { get; set; }

        public string GoogleDriveFileId { get; set; }

        [ForeignKey("StaffId")]
        public virtual Staff Staff { get; set; }
        [ForeignKey("TemplateId")]
        public virtual Template Template { get; set; }
        [ForeignKey("TemplateId")]
      //  public virtual Customer Customer { get; set; }
        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }
    }
}
