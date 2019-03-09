using HiEIS.Model;
using HiEIS.Service;
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

        public InvoiceController(IInvoiceService invoiceService, UserManager<MyUser> userManager, IFileService fileService, ITemplateService templateService, IPdfService pdfService)
        {
            _invoiceService = invoiceService;
            _userManager = userManager;
            _fileService = fileService;
            _templateService = templateService;
            _pdfService = pdfService;
        }

        [HttpGet("AmountInWord/{gNum}")]
        public ActionResult ToText(double gNum)
        {
            return Ok(new
            {
                value = _invoiceService.ConvertNumberToWord(gNum)
            });
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
                //Trang thai danh so
                invoice.Number = template.CurrentNo++.ToString();

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
