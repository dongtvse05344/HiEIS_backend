using HiEIS.Model;
using IronPdf;
using System;
using System.Collections.Generic;
using System.Text;

namespace HiEIS.Service
{
    public interface IPdfService
    {
        PdfDocument FillInInvoice(string fileUrl, Invoice invoice);
    }

    public class PdfService : IPdfService
    {
        public PdfDocument FillInInvoice(string fileUrl, Invoice invoice)
        {
            try
            {
                var pdf = PdfDocument.FromFile(fileUrl);
                var formFields = pdf.Form;
                
                formFields.GetFieldByName("Form").Value = invoice.Form;
                formFields.GetFieldByName("Serial").Value = invoice.Serial;
                formFields.GetFieldByName("Number").Value = invoice.Number.ToString();//***

                formFields.GetFieldByName("Day").Value = invoice.Date.Day.ToString();
                formFields.GetFieldByName("Month").Value = invoice.Date.Month.ToString();
                formFields.GetFieldByName("Year").Value = invoice.Date.Year.ToString();

                formFields.GetFieldByName("Name").Value = invoice.Name;
                formFields.GetFieldByName("Enterprise").Value = invoice.Enterprise;
                formFields.GetFieldByName("Address").Value = invoice.Address;
                formFields.GetFieldByName("Tel").Value = invoice.Tel;
                formFields.GetFieldByName("Fax").Value = invoice.Fax;
                formFields.GetFieldByName("BankAccountNumber").Value = invoice.BankAccountNumber;
                formFields.GetFieldByName("Bank").Value = invoice.Bank;
                formFields.GetFieldByName("PaymentMethod").Value = invoice.PaymentMethod.ToString();//***
                formFields.GetFieldByName("TaxNo").Value = invoice.TaxNo;

                int i = 0;
                foreach (var item in invoice.InvoiceItems)
                {
                    formFields.GetFieldByName("ProductName" + i).Value = item.Name;
                    formFields.GetFieldByName("Unit" + i).Value = item.Unit;
                    formFields.GetFieldByName("Quantity" + i).Value = item.Quantity.ToString();
                    formFields.GetFieldByName("UnitPrice" + i).Value = item.UnitPrice.ToString();//***
                    formFields.GetFieldByName("ProductTotal" + i).Value = item.Total.ToString();//***

                    i++;
                }

                formFields.GetFieldByName("VATRate").Value = invoice.VATRate.ToString();//***
                formFields.GetFieldByName("SubTotal").Value = invoice.SubTotal.ToString();//***
                formFields.GetFieldByName("VATAmount").Value = invoice.VATAmount.ToString();//***
                formFields.GetFieldByName("Total").Value = invoice.Total.ToString();//***
                formFields.GetFieldByName("AmountWords").Value = invoice.AmountInWords;
                
                return pdf;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
