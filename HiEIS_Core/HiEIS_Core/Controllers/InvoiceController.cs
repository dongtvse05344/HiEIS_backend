using HiEIS.Model;
using HiEIS.Service;
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

        public InvoiceController(IInvoiceService invoiceService, UserManager<MyUser> userManager)
        {
            _invoiceService = invoiceService;
            _userManager = userManager;
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
            try
            {
                var user = _userManager.GetUserAsync(User).Result;
                var invoice = model.Adapt<Invoice>();
                invoice.StaffId = user.Staff.Id;

                _invoiceService.CreateInvoice(invoice);
                _invoiceService.SaveChanges();
                return StatusCode(201, invoice.Id);
            }
            catch (Exception e)
            {
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
    }
}
