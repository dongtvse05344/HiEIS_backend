using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiEIS_Core.ViewModels
{
    public class InvoiceVM
    {
        public Guid Id { get; set; }
        //Mẫu số
        public string Form { get; set; }
        //Kí hiệu
        public string Serial { get; set; }
        public string Number { get; set; }
        //Loại HĐ (GTGT / Bán hàng)
        public int Type { get; set; }
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }
        public int PaymentMethod { get; set; }
        public int PaymentStatus { get; set; }

        //Cộng tiền hàng (chưa tính thuế)
        public float SubTotal { get; set; }
        public float VATRate { get; set; }
        public float VATAmount { get; set; }
        public float Total { get; set; }
        public string AmountInWords { get; set; }

        public string Note { get; set; }
        public Guid TemplateId { get; set; }
        public string StaffId { get; set; }
        public Guid CustomerId { get; set; }
    }

    public class InvoiceCM
    {
        public string Form { get; set; }
        //Kí hiệu
        public string Serial { get; set; }
        public string Number { get; set; }
        //Loại HĐ (GTGT / Bán hàng)
        public int Type { get; set; }
        public DateTime Date { get; set; }
        public int PaymentMethod { get; set; }
        public int PaymentStatus { get; set; }

        public string Note { get; set; }
        public Guid TemplateId { get; set; }
        public Guid CustomerId { get; set; }
    }

    public class InvoiceUM
    {
        public Guid Id { get; set; }
        public DateTime DueDate { get; set; }
        public int PaymentMethod { get; set; }
        public int PaymentStatus { get; set; }

        public string Note { get; set; }
    }
}
