using HiEIS.Model;
using HiEIS.Service;
using HiEIS_Core.Paging;
using HiEIS_Core.Utils;
using HiEIS_Core.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HiEIS_Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly UserManager<MyUser> _userManager;
        private readonly IFileService _fileService;
        private readonly ITemplateService _templateService;
        private readonly IPdfService _pdfService;
        private readonly IInvoiceItemService _invoiceItemService;
        private readonly ICurrentSignService _signService;
        private const string formatNumber = "0000000";
        private readonly ICompanyService _companyService;

        public InvoiceController(IInvoiceService invoiceService, UserManager<MyUser> userManager, IFileService fileService, ITemplateService templateService, IPdfService pdfService, IInvoiceItemService invoiceItemService, ICurrentSignService signService, ICompanyService companyService)
        {
            _invoiceService = invoiceService;
            _userManager = userManager;
            _fileService = fileService;
            _templateService = templateService;
            _pdfService = pdfService;
            _invoiceItemService = invoiceItemService;
            _signService = signService;
            _companyService = companyService;
        }

        [Authorize]
        [HttpGet]
        public ActionResult GetInvoices(string Enterprise ="",string TaxNo = "", int Month = -1, int Year = -1, int index = 1, int pageSize = 5)
        {
            try
            {
                var user = _userManager.GetUserAsync(User).Result;
                TaxNo = TaxNo != null ? TaxNo : "";
                Enterprise = Enterprise != null ? Enterprise : "";
                Month = (Month > 0 && Month <= 12) ? Month : DateTime.Now.Month;
                Year = Year > 2016 ? Year : DateTime.Now.Year;

                var invoices = _invoiceService.GetInvoices(_ =>
                            _.Staff.CompanyId.Equals(user.Staff.CompanyId) &&
                            _.TaxNo.Contains(TaxNo) &&
                            _.Enterprise.Contains(Enterprise) &&
                            _.Date.Month.Equals(Month) &&
                            _.Date.Year.Equals(Year)
                            ).OrderByDescending(_=>_.Date);
                var result = invoices.ToPageList<InvoiceVM, Invoice>(index, pageSize);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}/file")]
        public ActionResult GetInvoiceFile(Guid id)
        {
            try
            {
                var invoice = _invoiceService.GetInvoice(id);
                if (invoice == null) return NotFound();

                var file = _fileService.GetFile(invoice.FileUrl).Result;
                return File(file.Stream, file.ContentType);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult GetInvoice(Guid id)
        {
            try
            {
                var invoice = _invoiceService.GetInvoice(id);
                if (invoice == null) return NotFound();

                var result = invoice.Adapt<InvoiceUM>();
                result.InvoiceItemCMs = new List<InvoiceItemCM>();
                foreach (var item in invoice.InvoiceItems)
                {
                    result.InvoiceItemCMs.Add(item.Adapt<InvoiceItemCM>());
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult CreateInvoice(InvoiceCM model)
        {
            string fileUrl = null;
            try
            {
                var user = _userManager.GetUserAsync(User).Result;
                var invoice = model.Adapt<Invoice>();
                invoice.InvoiceItems = new List<InvoiceItem>();
                foreach (var item in model.InvoiceItemCMs)
                {
                    invoice.InvoiceItems.Add(item.Adapt<InvoiceItem>());
                }
                invoice.StaffId = user.Id;
                var template = _templateService.GetTemplate(model.TemplateId);

                invoice.Form = template.Form;
                invoice.Serial = template.Serial;
                if (invoice.Type == (int)InvoiceType.Approve)
                {
                    //Trang thai danh so
                    invoice.Number = template.CurrentNo++.ToString(formatNumber);
                }
                else
                {
                    invoice.Type = (int)InvoiceType.New;
                }
                string fileName = _fileService.GenerateFileName("Files/" + user.Staff.CompanyId + "/Invoice/" + invoice.TaxNo + ".pdf");

                invoice.FileUrl = _invoiceService.GenerateFinalPdf(fileName, invoice, template.FileUrl);
                fileUrl = invoice.FileUrl;
                _invoiceService.CreateInvoice(invoice);
                _invoiceService.SaveChanges();
                invoice.LockupCode = invoice.No.ToString("000000");
                _invoiceService.SaveChanges();
                return StatusCode(201, invoice.Id);
            }
            catch (Exception e)
            {
                if (fileUrl != null)
                {
                    _fileService.DeleteFile(fileUrl);
                }

                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPut]
        public ActionResult UpdateInvoice(InvoiceUM model)
        {
            string fileUrl = null;
            try
            {
                _invoiceItemService.DeleteInvoiceItem(_ => _.InvoiceId.Equals(model.Id));
                var invoice = _invoiceService.GetInvoice(model.Id);
                if (invoice == null) return NotFound();
                var user = _userManager.GetUserAsync(User).Result;

                var oldFile = invoice.FileUrl;

                invoice = model.Adapt(invoice);
                invoice.InvoiceItems = new List<InvoiceItem>();
                foreach (var item in model.InvoiceItemCMs)
                {
                    invoice.InvoiceItems.Add(item.Adapt<InvoiceItem>());
                }
                var template = _templateService.GetTemplate(model.TemplateId);

                invoice.Form = template.Form;
                invoice.Serial = template.Serial;
                if (invoice.Type == (int)InvoiceType.Approve)
                {
                    //Trang thai danh so
                    invoice.Number = template.CurrentNo++.ToString(formatNumber);
                }
                else
                {
                    invoice.Type = (int)InvoiceType.New;
                }

                string fileName = _fileService.GenerateFileName("Files/" + user.Staff.CompanyId + "/" + nameof(FileType.Invoice) + "/" + invoice.TaxNo + ".pdf");
                invoice.FileUrl = _invoiceService.GenerateFinalPdf(fileName, invoice, template.FileUrl);
                fileUrl = invoice.FileUrl;

                _invoiceService.SaveChanges();
                _fileService.DeleteFile(oldFile);
                return StatusCode(200);
            }
            catch (Exception e)
            {
                if (fileUrl != null)
                {
                    _fileService.DeleteFile(fileUrl);
                }

                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPut("{id}/Approve")]
        public ActionResult ApproveInvoice(Guid id)
        {
            string fileUrl = null;
            try
            {
                var user = _userManager.GetUserAsync(User).Result;
                var invoice = _invoiceService.GetInvoice(id);
                if (invoice == null) return NotFound();
                var oldFile = invoice.FileUrl;
                invoice.Type = (int)InvoiceType.Approve;
                invoice.Number = invoice.Template.CurrentNo++.ToString(formatNumber);

                string fileName = _fileService.GenerateFileName("Files/" + user.Staff.CompanyId + "/" + nameof(FileType.Invoice) + "/" + invoice.TaxNo + ".pdf");
                invoice.FileUrl = _invoiceService.GenerateFinalPdf(fileName, invoice, invoice.Template.FileUrl);
                fileUrl = invoice.FileUrl;

                _invoiceService.SaveChanges();
                _fileService.DeleteFile(oldFile);
                return StatusCode(200);
            }
            catch (Exception e)
            {
                if (fileUrl != null)
                {
                    _fileService.DeleteFile(fileUrl);
                }
                return BadRequest(e.Message);
            }
        }

        [HttpGet("AmountInWord/{gNum}")]
        public ActionResult ToText(double gNum)
        {
            return Ok(new
            {
                value = _invoiceService.ConvertNumberToWord(gNum)
            });
        }

        [Authorize]
        [HttpDelete("{id}")]
        public ActionResult Delete(Guid id)
        {
            try
            {
                var invoice = _invoiceService.GetInvoice(id);
                if (invoice == null) return NotFound();
                if (invoice.Type == (int)InvoiceType.New)
                {
                    _invoiceItemService.DeleteInvoiceItem(_ => _.InvoiceId.Equals(invoice.Id));
                    _invoiceService.DeleteInvoice(invoice);
                    _invoiceService.SaveChanges();
                    return Ok();
                }
                else
                {
                    return BadRequest("Invoice is not new to delete");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("Sign")]
        public ActionResult Sign()
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPost("Cancel")]
        public ActionResult Cancel(Guid id)
        {
            try
            {
                var invoice = _invoiceService.GetInvoice(id);
                if (invoice == null) return NotFound();
                if (invoice.Type == (int)InvoiceType.New) return BadRequest("Không thể hủy hóa đơn này!");

                invoice.Type = (int)InvoiceType.Reject;
                _invoiceService.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("ReceiveInvoiceSigned")]
        public async Task<ActionResult> ReceiveSignedInvoice([FromForm]InvoiceSigned models)
        {
            var currentCode = _signService.GetCurrentSigns(_ => _.Code.Equals(models.Code)).FirstOrDefault();
            if (currentCode == null) return BadRequest();
           // string fileName = _fileService.GenerateFileName("Files/" + company.Id + "/Invoice/" + invoice.TaxNo + ".pdf");
            foreach (var item in models.FileContents)
            {
                string newUrl = null;
                try
                {
                    var invoiceId = Path.GetFileNameWithoutExtension(item.FileName);
                    var invoice = _invoiceService.GetInvoice(Guid.Parse(invoiceId));
                    if (invoice == null) continue;
                    string fileName = _fileService.GenerateFileName(invoice.LockupCode +".pdf");
                    var oldUrl = invoice.FileUrl;
                    invoice.FileUrl = _fileService.SaveFile(currentCode.CompanyId.ToString(), nameof(FileType.Invoice), item,fileName).Result;
                    invoice.Type = (int)InvoiceType.Signed;
                    newUrl = invoice.FileUrl;
                    _invoiceService.SaveChanges();
                    _fileService.DeleteFile(oldUrl);
                }
                catch (Exception e)
                {
                    if (newUrl != null)
                    {
                        _fileService.DeleteFile(newUrl);
                    }
                    continue;
                }
            }
            return Ok();
        }

        [Authorize]
        [HttpGet("SearchInvoices")]
        public ActionResult SearchInvoices(string companyName = "", DateTime? fromDate = null, DateTime? toDate = null, int index = 1, int pageSize = 5)
        {
            try
            {
                var user = _userManager.GetUserAsync(User).Result;

                var invoices = _invoiceService.GetInvoices(_ => _.Enterprise.Contains(companyName)
                                                && _.Staff.CompanyId.Equals(user.Staff.CompanyId));
                if (fromDate != null)
                    invoices = invoices.Where(_ => _.Date.Date >= fromDate.Value.Date);
                if (toDate != null)
                    invoices = invoices.Where(_ => _.Date.Date <= toDate.Value.Date);
                invoices = invoices.OrderByDescending(_ => _.Date);

                var result = invoices.ToPageList<InvoiceVM, Invoice>(index, pageSize);

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
