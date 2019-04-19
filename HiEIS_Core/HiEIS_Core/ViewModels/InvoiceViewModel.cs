using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiEIS_Core.ViewModels
{
    public class InvoiceVM
    {
        public Guid Id { get; set; }
        public string Enterprise { get; set; }
        public string Number { get; set; }
        //Loại HĐ (GTGT / Bán hàng)
        public int Type { get; set; }
        public DateTime DateCreated { get; set; }
        public int PaymentStatus { get; set; }
        public float Total { get; set; }
        public string Note { get; set; }
    }

    public class InvoiceSigned
    {
        public string Code { get; set; }
        public List<IFormFile> FileContents { get; set; }
    }

    public class InvoiceCM
    {
        //Loại HĐ (GTGT / Bán hàng)
        public int Type { get; set; }
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }
        public string FileUrl { get; set; }
        //Thông tin khách hàng
        public string Name { get; set; }
        public string Enterprise { get; set; }
        public string[] Email { get; set; }
        public string TaxNo { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }
        public string Fax { get; set; }
        public string Bank { get; set; }
        public string BankAccountNumber { get; set; }
        public string PaymentMethod { get; set; }
        public int PaymentStatus { get; set; }

        //Cộng tiền hàng (chưa tính thuế)
        public float SubTotal { get; set; }
        public float VATRate { get; set; }
        public float VATAmount { get; set; }
        public float Total { get; set; }
        public string AmountInWords { get; set; }

        public string Note { get; set; }
        public Guid TemplateId { get; set; }

        public List<InvoiceItemCM> InvoiceItemCMs { get; set; }
    }

    public class InvoiceUM
    {
        public Guid Id { get; set; }
        //Loại HĐ (GTGT / Bán hàng)
        public int Type { get; set; }
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }

        //Thông tin khách hàng
        public string Name { get; set; }
        public string Enterprise { get; set; }
        public string TaxNo { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }
        public string Fax { get; set; }
        public string Bank { get; set; }
        public string BankAccountNumber { get; set; }
        public string PaymentMethod { get; set; }
        public int PaymentStatus { get; set; }

        //Cộng tiền hàng (chưa tính thuế)
        public float SubTotal { get; set; }
        public float VATRate { get; set; }
        public float VATAmount { get; set; }
        public float Total { get; set; }
        public string AmountInWords { get; set; }

        public string Note { get; set; }
        public Guid TemplateId { get; set; }

        public List<InvoiceItemCM> InvoiceItemCMs { get; set; }
    }
    
    public class InvoiceUploadFileVM
    {
        public string GoogleDriveFolderId { get; set; }
        public Guid InvoiceID { get; set; }
    }
}
