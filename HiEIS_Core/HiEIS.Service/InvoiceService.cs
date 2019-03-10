using HiEIS.Data.Infrastructures;
using HiEIS.Data.Repositories;
using HiEIS.Model;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Mapster;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace HiEIS.Service
{
    public interface IInvoiceService
    {
        IQueryable<Invoice> GetInvoices();
        IQueryable<Invoice> GetInvoices(Expression<Func<Invoice, bool>> where);
        Invoice GetInvoice(Guid id);
        void CreateInvoice(Invoice invoice);
        void UpdateInvoice(Invoice invoice);
        void DeleteInvoice(Invoice invoice);
        string ConvertNumberToWord(double gNum);
        string GenerateFinalPdf(string filename, Invoice invoice, string fileTemplate);
        void SaveChanges();
    }

    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public InvoiceService(IInvoiceRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public void CreateInvoice(Invoice invoice)
        {
            _repository.Add(invoice);
        }

        public void DeleteInvoice(Invoice invoice)
        {
            _repository.Delete(invoice);
        }

        public Invoice GetInvoice(Guid id)
        {
            return _repository.GetById(id);
        }

        public IQueryable<Invoice> GetInvoices()
        {
            return _repository.GetAll();
        }

        public IQueryable<Invoice> GetInvoices(Expression<Func<Invoice, bool>> where)
        {
            return _repository.GetMany(where);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void UpdateInvoice(Invoice invoice)
        {
            _repository.Update(invoice);
        }


        private string Chu(string gNumber)
        {
            string result = "";
            switch (gNumber)
            {
                case "0":
                    result = "không";
                    break;
                case "1":
                    result = "một";
                    break;
                case "2":
                    result = "hai";
                    break;
                case "3":
                    result = "ba";
                    break;
                case "4":
                    result = "bốn";
                    break;
                case "5":
                    result = "năm";
                    break;
                case "6":
                    result = "sáu";
                    break;
                case "7":
                    result = "bảy";
                    break;
                case "8":
                    result = "tám";
                    break;
                case "9":
                    result = "chín";
                    break;
            }
            return result;
        }

        private  string Donvi(string so)
        {
            string Kdonvi = "";

            if (so.Equals("1"))
                Kdonvi = "";
            if (so.Equals("2"))
                Kdonvi = "nghìn";
            if (so.Equals("3"))
                Kdonvi = "triệu";
            if (so.Equals("4"))
                Kdonvi = "tỷ";
            if (so.Equals("5"))
                Kdonvi = "nghìn tỷ";
            if (so.Equals("6"))
                Kdonvi = "triệu tỷ";
            if (so.Equals("7"))
                Kdonvi = "tỷ tỷ";

            return Kdonvi;
        }

        private  string Tach(string tach3)
        {
            string Ktach = "";
            if (tach3.Equals("000"))
                return "";
            if (tach3.Length == 3)
            {
                string tr = tach3.Trim().Substring(0, 1).ToString().Trim();
                string ch = tach3.Trim().Substring(1, 1).ToString().Trim();
                string dv = tach3.Trim().Substring(2, 1).ToString().Trim();
                if (tr.Equals("0") && ch.Equals("0"))
                    Ktach = " không trăm lẻ " + Chu(dv.ToString().Trim()) + " ";
                if (!tr.Equals("0") && ch.Equals("0") && dv.Equals("0"))
                    Ktach = Chu(tr.ToString().Trim()).Trim() + " trăm ";
                if (!tr.Equals("0") && ch.Equals("0") && !dv.Equals("0"))
                    Ktach = Chu(tr.ToString().Trim()).Trim() + " trăm lẻ " + Chu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi " + Chu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && dv.Equals("0"))
                    Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi ";
                if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && dv.Equals("5"))
                    Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi lăm ";
                if (tr.Equals("0") && ch.Equals("1") && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = " không trăm mười " + Chu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && ch.Equals("1") && dv.Equals("0"))
                    Ktach = " không trăm mười ";
                if (tr.Equals("0") && ch.Equals("1") && dv.Equals("5"))
                    Ktach = " không trăm mười lăm ";
                if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi " + Chu(dv.Trim()).Trim() + " ";
                if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && dv.Equals("0"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi ";
                if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi lăm ";
                if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm mười " + Chu(dv.Trim()).Trim() + " ";

                if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && dv.Equals("0"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm mười ";
                if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm mười lăm ";

            }


            return Ktach;

        }

        public string ConvertNumberToWord(double gNum)
        {
            if (gNum == 0)
                return "Không đồng";

            string lso_chu = "";
            string tach_mod = "";
            string tach_conlai = "";
            double Num = Math.Round(gNum, 0);
            string gN = Convert.ToString(Num);
            int m = Convert.ToInt32(gN.Length / 3);
            int mod = gN.Length - m * 3;
            string dau = "[+]";

            // Dau [+ , - ]
            if (gNum < 0)
                dau = "[-]";
            dau = "";

            // Tach hang lon nhat
            if (mod.Equals(1))
                tach_mod = "00" + Convert.ToString(Num.ToString().Trim().Substring(0, 1)).Trim();
            if (mod.Equals(2))
                tach_mod = "0" + Convert.ToString(Num.ToString().Trim().Substring(0, 2)).Trim();
            if (mod.Equals(0))
                tach_mod = "000";
            // Tach hang con lai sau mod :
            if (Num.ToString().Length > 2)
                tach_conlai = Convert.ToString(Num.ToString().Trim().Substring(mod, Num.ToString().Length - mod)).Trim();

            ///don vi hang mod
            int im = m + 1;
            if (mod > 0)
                lso_chu = Tach(tach_mod).ToString().Trim() + " " + Donvi(im.ToString().Trim());
            /// Tach 3 trong tach_conlai

            int i = m;
            int _m = m;
            int j = 1;
            string tach3 = "";
            string tach3_ = "";

            while (i > 0)
            {
                tach3 = tach_conlai.Trim().Substring(0, 3).Trim();
                tach3_ = tach3;
                lso_chu = lso_chu.Trim() + " " + Tach(tach3.Trim()).Trim();
                m = _m + 1 - j;
                if (!tach3_.Equals("000"))
                    lso_chu = lso_chu.Trim() + " " + Donvi(m.ToString().Trim()).Trim();
                tach_conlai = tach_conlai.Trim().Substring(3, tach_conlai.Trim().Length - 3);

                i = i - 1;
                j = j + 1;
            }
            if (lso_chu.Trim().Substring(0, 1).Equals("k"))
                lso_chu = lso_chu.Trim().Substring(10, lso_chu.Trim().Length - 10).Trim();
            if (lso_chu.Trim().Substring(0, 1).Equals("l"))
                lso_chu = lso_chu.Trim().Substring(2, lso_chu.Trim().Length - 2).Trim();
            if (lso_chu.Trim().Length > 0)
                lso_chu = dau.Trim() + " " + lso_chu.Trim().Substring(0, 1).Trim().ToUpper() + lso_chu.Trim().Substring(1, lso_chu.Trim().Length - 1).Trim() ;

            return lso_chu.ToString().Trim();

        }

        public string GenerateFinalPdf(string fileName, Invoice invoice, string fileTemplate)
        {
            var pages = Math.Ceiling(invoice.InvoiceItems.Count / 10d);
            var temp = invoice.InvoiceItems;
            var pdf = invoice;
            List<string> generatedFiles = new List<string>();

            //pdf = invoice.Adapt<Invoice>();
            fileName = fileName.Replace(".pdf", ""); //remove extension

            for (int i = 0; i < pages; i++)
            {
                bool isLastPage = (i == (pages - 1)) ? true : false;
                pdf.InvoiceItems = temp
                    .Skip(10 * i)
                    .Take(10)
                    .ToList();
                string newFile = fileName + "_" + i + ".pdf";
                FillDataPdf(newFile, pdf, isLastPage, fileTemplate);

                string newFileUrl = Path.Combine(Directory.GetCurrentDirectory(), newFile);
                generatedFiles.Add(newFileUrl);
                invoice.InvoiceItems = temp;
            }
            return MergePdfFiles(generatedFiles, fileName + ".pdf");
        }

        private string MergePdfFiles(List<string> files, string outputFile)
        {
            try
            {
                string fileUrl = Path.Combine(Directory.GetCurrentDirectory(), outputFile);
                Document document = new Document();
                PdfCopy copy = new PdfCopy(document, new FileStream(fileUrl, FileMode.Create));
                PdfReader reader;

                document.Open();
                for (int i = 0; i < files.Count; i++)
                {
                    if (File.Exists(files[i]))
                    {
                        //create PdfReader object
                        reader = new PdfReader(files[i]);

                        //merge combine pages
                        for (int page = 1; page <= reader.NumberOfPages; page++)
                            copy.AddPage(copy.GetImportedPage(reader, page));
                        reader.Close();
                    }
                }
                document.Close();

                foreach (var file in files)
                {
                    //delete the chosen file
                    File.Delete(file);
                }

            }
            catch (Exception)
            {

                throw;
            }
            return outputFile;
        }
        private void FillDataPdf(string fileName, Invoice invoice, bool IsLastPage, string fileTemplate)
        {
            fileTemplate = Path.Combine(Directory.GetCurrentDirectory(), fileTemplate);
            fileName = Path.Combine(Directory.GetCurrentDirectory(), fileName);

            using (FileStream outputFile = new FileStream(fileName, FileMode.Create))
            {
                try
                {
                    PdfReader pdfReader = new PdfReader(fileTemplate);
                    PdfStamper pdfStamper = new PdfStamper(pdfReader, outputFile);
                    var fontPath = Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\times.ttf";
                    BaseFont unicode =
                       BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    // set Text Size

                    AcroFields fields = pdfStamper.AcroFields;
                    fields.SetFieldProperty("Address", "textsize", (float)8, null);
                    fields.SetFieldProperty("Enterprise", "textsize", (float)8, null);
                    fields.SetFieldProperty("Enterprise", "textfont", unicode, null);
                    fields.SetFieldProperty("Address", "textfont", unicode, null);
                    fields.SetFieldProperty("Name", "textfont", unicode, null);
                    fields.SetFieldProperty("PaymentMethod", "textfont", unicode, null);
                    fields.SetFieldProperty("Day", "textfont", unicode, null);
                    fields.SetFieldProperty("Month", "textfont", unicode, null);
                    fields.SetFieldProperty("Year", "textfont", unicode, null);
                    fields.SetFieldProperty("Bank", "textfont", unicode, null);
                    fields.SetFieldProperty("TaxNo", "textfont", unicode, null);
                    fields.SetFieldProperty("BankAccountNumber", "textfont", unicode, null);
                    fields.SetFieldProperty("Tel", "textfont", unicode, null);
                    fields.SetFieldProperty("Fax", "textfont", unicode, null);

                    fields.SetField("Form", invoice.Form);
                    fields.SetField("Serial", invoice.Serial);
                    fields.SetField("Number", invoice.Number == null ? "" : invoice.Number.ToString());

                    fields.SetField("Day", invoice.Date.Day.ToString());
                    fields.SetField("Month", invoice.Date.Month.ToString());
                    fields.SetField("Year", invoice.Date.Year.ToString());

                    fields.SetField("Name", invoice.Name);
                    fields.SetField("Enterprise", invoice.Enterprise);
                    fields.SetField("Address", invoice.Address);
                    fields.SetField("Tel", invoice.Tel);
                    fields.SetField("Fax", invoice.Fax);
                    fields.SetField("BankAccountNumber", invoice.BankAccountNumber);
                    fields.SetField("Bank", invoice.Bank);
                    fields.SetField("PaymentMethod", invoice.PaymentMethod);

                    for (int i = 0; i < invoice.InvoiceItems.Count; i++)
                    {
                        var item = invoice.InvoiceItems.ElementAt(i);
                        fields.SetFieldProperty("ProductName" + i, "textsize", (float)10, null);
                        fields.SetField("ProductName" + i, item.Name);

                        fields.SetField("Unit" + i, item.Unit);
                        fields.SetField("Quantity" + i, item.Quantity.ToString());
                        fields.SetField("UnitPrice" + i, item.UnitPrice.ToString("#,##0"));
                        fields.SetField("ProductTotal" + i, item.Total.ToString("#,##0"));
                    }
                    if (IsLastPage)
                    {
                        fields.SetField("TaxNo", invoice.TaxNo);
                        fields.SetField("VATRate", invoice.VATRate == -1 ? "x" : (invoice.VATRate).ToString());
                        fields.SetField("SubTotal", invoice.SubTotal.ToString("#,##0"));
                        fields.SetField("VATAmount", invoice.VATRate == -1 ? "x":invoice.VATAmount.ToString("#,##0"));
                        fields.SetField("Total", invoice.Total.ToString("#,##0"));
                        fields.SetField("AmountWords", invoice.AmountInWords);
                    }

                    PdfContentByte overContent = pdfStamper.GetOverContent(1);
                    pdfStamper.FormFlattening = true;
                    pdfStamper.Close();
                    pdfReader.Close();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}
