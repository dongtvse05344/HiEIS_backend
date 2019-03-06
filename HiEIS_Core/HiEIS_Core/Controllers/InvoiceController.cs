using HiEIS.Model;
using HiEIS.Service;
using HiEIS_Core.Utils;
using HiEIS_Core.ViewModels;
using Mapster;
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

        public InvoiceController(IInvoiceService invoiceService, UserManager<MyUser> userManager, IFileService fileService, ITemplateService templateService, IPdfService pdfService)
        {
            _invoiceService = invoiceService;
            _userManager = userManager;
            _fileService = fileService;
            _templateService = templateService;
            _pdfService = pdfService;
        }

        [HttpGet("{id}")]
        public ActionResult GetInvoice(Guid id)
        {
            try
            {
                var invoice = _invoiceService.GetInvoice(id);
                if (invoice == null) return NotFound();
                return Ok(invoice.Adapt<InvoiceVM>());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public ActionResult CreateInvoice(InvoiceCM model)
        {
            string fileUrl = null;
            try
            {
                var user = _userManager.GetUserAsync(User).Result;
                var invoice = model.Adapt<Invoice>();
                var template = _templateService.GetTemplate(model.TemplateId);

                invoice.Form = template.Form;
                invoice.Serial = template.Serial;
                invoice.Number = template.CurrentNo++;

                var pdfSupport = new PdfSupport();
                for (int i = 0; i < model.InvoiceItemCMs.Count; i += 10)
                {
                    List<InvoiceItem> invoiceItems = new List<InvoiceItem>();
                    for (int j = i; j < i + 10 || j >= model.InvoiceItemCMs.Count; j++)
                    {
                        invoiceItems.Add(model.InvoiceItemCMs[j].Adapt<InvoiceItem>());
                    }
                    invoice.InvoiceItems = invoiceItems;

                    pdfSupport.PdfDocuments.Add(_pdfService.FillInInvoice(template.FileUrl, invoice));
                }

                invoice.FileUrl = _fileService.SaveFile(user.Staff.CompanyId.ToString(), nameof(FileType.Invoice), pdfSupport.PdfDocuments, template.CurrentNo);
                
                invoice.StaffId = user.Staff.Id;
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

        [HttpPut]
        public ActionResult UpdateInvoice(InvoiceUM model)
        {
            try
            {
                var invoice = _invoiceService.GetInvoice(model.Id);
                if (invoice == null) return NotFound();
                invoice = model.Adapt(invoice);
                _invoiceService.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public ActionResult GetInvoices()
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
    }
}
