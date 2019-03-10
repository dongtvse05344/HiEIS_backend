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
using System.Linq;
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
        private const string  formatNumber = "0000000";
        public InvoiceController(IInvoiceService invoiceService, UserManager<MyUser> userManager, IFileService fileService, ITemplateService templateService, IPdfService pdfService, IInvoiceItemService invoiceItemService)
        {
            _invoiceService = invoiceService;
            _userManager = userManager;
            _fileService = fileService;
            _templateService = templateService;
            _pdfService = pdfService;
            _invoiceItemService = invoiceItemService;
        }

        [Authorize]
        [HttpGet]
        public ActionResult GetInvoices(string TaxNo = "", int Month = -1, int Year = -1, int index = 1, int pageSize = 5)
        {
            try
            {
                var user = _userManager.GetUserAsync(User).Result;
                TaxNo = TaxNo != null ? TaxNo : "";
                Month = (Month > 0 && Month <= 12) ? Month : DateTime.Now.Month;
                Year = Year > 2016 ? Year : DateTime.Now.Year;

                var invoices = _invoiceService.GetInvoices(_ =>
                            _.Staff.CompanyId.Equals(user.Staff.CompanyId) &&
                            _.TaxNo.Contains(TaxNo) &&
                            _.Date.Month.Equals(Month) &&
                            _.Date.Year.Equals(Year)
                            );
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
                if(invoice.Type == (int)InvoiceType.Approve)
                {
                    //Trang thai danh so
                    invoice.Number = template.CurrentNo++.ToString(formatNumber);
                }
                else
                {
                    invoice.Type = (int)InvoiceType.New;
                }
                string fileName = _fileService.GenerateFileName("Files/"+user.Staff.CompanyId+"/Invoice/"+invoice.TaxNo +".pdf");

                invoice.FileUrl = _invoiceService.GenerateFinalPdf(fileName, invoice, template.FileUrl);
                fileUrl = invoice.FileUrl;
                _invoiceService.CreateInvoice(invoice);
                
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
            string fileUrl =null;
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

                string fileName = _fileService.GenerateFileName("Files/" + user.Staff.CompanyId + "/"+ nameof(FileType.Invoice) + "/" + invoice.TaxNo + ".pdf");
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
                if(invoice.Type == (int) InvoiceType.New)
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
    }
}
