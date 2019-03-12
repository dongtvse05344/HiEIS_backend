using Hangfire;
using HiEIS.Model;
using HiEIS.Service;
using HiEIS_Core.Utils;
using HiEIS_Core.ViewModels;
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
    public class CurrentSignController : ControllerBase
    {
        private readonly UserManager<MyUser> _userManager;
        private readonly ICurrentSignService _currentSignService;
        private readonly IInvoiceService _invoiceService;

        public CurrentSignController(UserManager<MyUser> userManager, ICurrentSignService currentSignService, IInvoiceService invoiceService)
        {
            _userManager = userManager;
            _currentSignService = currentSignService;
            _invoiceService = invoiceService;
        }
        
        [Authorize]
        [HttpPost("GenerateCode")]
        public ActionResult GenerateCode()
        {
            try
            {
                var user = _userManager.GetUserAsync(User).Result;
                var companyId = user.Staff.CompanyId;
                var currentSign = _currentSignService.GetCurrentSigns(_ => _.CompanyId == companyId).FirstOrDefault();
                if (currentSign != null) return BadRequest("Mã đã được tạo!");
                
                Random random = new Random();
                string code;
                do
                {
                    code = "";
                    for (int i = 0; i < 4; i++)
                    {
                        code += random.Next() % 10;
                    }
                } while (_currentSignService.GetCurrentSigns(_ => _.Code.Equals(code)).FirstOrDefault() != null);

                currentSign = new CurrentSign
                {
                    Code = code,
                    CompanyId = companyId,
                    DateExpiry = DateTime.Now.AddHours(1)
                };

                _currentSignService.CreateCurrentSign(currentSign);
                BackgroundJob.Schedule(() => this.Delete(code)
                , TimeSpan.FromHours(1));
                _currentSignService.SaveChanges();

                return StatusCode(201, code);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [Authorize]
        [HttpPost("RegenerateCode")]
        public ActionResult RegenerateCode()
        {
            try
            {
                var user = _userManager.GetUserAsync(User).Result;
                var companyId = user.Staff.CompanyId;
                var currentSign = _currentSignService.GetCurrentSigns(_ => _.CompanyId == companyId).FirstOrDefault();

                Random random = new Random();
                string code = "";
                do
                {
                    for (int i = 0; i < 4; i++)
                    {
                        code += random.Next() % 10;
                    }
                } while (_currentSignService.GetCurrentSigns(_ => _.Code.Equals(code)).FirstOrDefault() != null);

                currentSign.Code = code;
                currentSign.DateExpiry = DateTime.Now.AddHours(1);
                BackgroundJob.Schedule(()=> this.Delete(code)
                , TimeSpan.FromHours(1));
                _currentSignService.UpdateCurrentSign(currentSign);
                _currentSignService.SaveChanges();

                return StatusCode(201, code);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        public void Delete(string code)
        {
            var currentSign = _currentSignService.GetCurrentSigns(_ => _.Code.Equals(code)).FirstOrDefault();
            _currentSignService.DeleteCurrentSign(currentSign);
            _currentSignService.SaveChanges();
        }

        [HttpGet("ApproveInvoices")]
        public ActionResult GetInvoices(string code)
        {
            try
            {
                var currentSign = _currentSignService.GetCurrentSigns(_ => _.Code.Equals(code)).FirstOrDefault();
                if (currentSign == null) return BadRequest("Nhập mã sai!");
                if (currentSign.DateExpiry < DateTime.Now) return BadRequest("Mã đã hết hạn!");

                var companyId = currentSign.CompanyId;
                var invoices = _invoiceService.GetInvoices(_ => _.Template.CompanyId == companyId && _ .Type == (int)InvoiceType.Approve);
                if (invoices == null) return NotFound();

                List<CurrentSignVM> result = new List<CurrentSignVM>();
                foreach (var item in invoices)
                {
                    result.Add(new CurrentSignVM
                    {
                        Id = item.Id,
                        Path = item.FileUrl
                    });
                }

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
