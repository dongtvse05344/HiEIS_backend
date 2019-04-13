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
using System.Text;
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
        private readonly IEmailService _mailService;
        private readonly ICustomerService _customerService;

        public InvoiceController(IInvoiceService invoiceService, UserManager<MyUser> userManager, IFileService fileService, ITemplateService templateService, IPdfService pdfService, IInvoiceItemService invoiceItemService, ICurrentSignService signService, ICompanyService companyService, IEmailService mailService, ICustomerService customerService)
        {
            _invoiceService = invoiceService;
            _userManager = userManager;
            _fileService = fileService;
            _templateService = templateService;
            _pdfService = pdfService;
            _invoiceItemService = invoiceItemService;
            _signService = signService;
            _companyService = companyService;
            _mailService = mailService;
            _customerService = customerService;
        }

        [Authorize]
        [HttpGet]
        public ActionResult SearchInvoices(string companyName = "", DateTime? fromDate = null, DateTime? toDate = null, int index = 1, int pageSize = 5)
        {
            try
            {
                var user = _userManager.GetUserAsync(User).Result;

                var invoices = _invoiceService.GetInvoices(
                                                    _ => _.Enterprise.Contains(companyName)
                                                &&  _.Staff.CompanyId.Equals(user.Staff.CompanyId));
                if (fromDate != null)
                    invoices = invoices.Where(_ => _.DateCreated.Date >= fromDate.Value.Date);
                if (toDate != null)
                    invoices = invoices.Where(_ => _.DateCreated.Date <= toDate.Value.Date);
                invoices = invoices.OrderByDescending(_ => _.DateCreated);

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
                CustomerCM customer = invoice.Adapt<CustomerCM>();
                Hangfire.BackgroundJob.Enqueue(() => AddOrUpdateCustomer(customer));
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

        [HttpPatch]
        public void AddOrUpdateCustomer(CustomerCM customer)
        {
            var customerDb = _customerService.GetCustomers().FirstOrDefault(_ => _.TaxNo.Equals(customer.TaxNo));
            if(customerDb ==null)
            {
                _customerService.CreateCustomer(customer.Adapt<Customer>());
            }
            else
            {
                customerDb = customer.Adapt(customerDb);
                _customerService.UpdateCustomer(customerDb);
            }
            _customerService.SaveChanges();
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

                //Send mail
                Hangfire.BackgroundJob.Enqueue(() => SendMail(new Invoice()
                {
                    FileUrl = invoice.FileUrl  // Link file to send
                }));
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

        public void SendMail(Invoice invoice)
        {
            var fileAttachments = new List<FileAttachmentModel>();
            fileAttachments.Add(new FileAttachmentModel
            {
                FileName = "Hóa đơn tháng " + DateTime.Now.Month.ToString() + ".pdf",
                FileContentStream = _fileService.GetFile(invoice.FileUrl).Result.Stream
            });
            var mailModel = new EmailModel()
            {
                FileAttachments = fileAttachments,
                ToMail = "xhunter1412@gmail.com"
            };
            _mailService.SendEmail(mailModel);
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
                }
            }
            return Ok();
        }

        /*
        [HttpPost("UploadFolder")]
        public ActionResult UploadFolder([FromBody]string access_token)
        {
            try
            {
                string url = "https://www.googleapis.com/drive/v3/files";
                
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);
                    HttpContent content = new StringContent("{\"mimeType\": \"application/vnd.google-apps.folder\", \"name\": \"Invoices\"}", Encoding.UTF8, "application/json");
                    var response = httpClient.PostAsync(url, content).Result;

                    return Ok(response.Content.ReadAsStringAsync().Result);
                }
                
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        */

        [HttpPost("UploadFile")]
        public async Task<ActionResult> UploadFile([FromBody]InvoiceUploadFileVM model)
        {
            try
            {
                string url = "https://www.googleapis.com/upload/drive/v3/files?" 
                        + "uploadType=multipart&"
                        + "fields=id";
                var invoice = _invoiceService.GetInvoice(model.InvoiceID);
                if (invoice == null) return NotFound();
                
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + model.Access_token);

                    ByteArrayContent fileContent;
                    using (var stream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), invoice.FileUrl), FileMode.Open))
                    {
                        using (var binaryReader = new BinaryReader(stream))
                        {
                            fileContent = new ByteArrayContent(binaryReader.ReadBytes((int)stream.Length));
                            fileContent.Headers.Add("Content-Type", "application/pdf");
                            fileContent.Headers.Add("Content-Length", stream.Length.ToString());
                        }
                    }

                    var googleDriveFileVM = new GoogleDriveUploadFileVM
                    {
                        name = invoice.Enterprise + ".pdf",
                        title = invoice.Enterprise,
                        parents = new List<string> { model.GoogleDriveFolderId }
                    };
                    StringContent fileInfoContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(googleDriveFileVM), Encoding.UTF8, "application/json");

                    MultipartContent content = new MultipartContent("related", "HiEIS")
                    {
                        fileInfoContent,
                        fileContent
                    };
                    /*
                    content.Headers.Remove("Content-Type");
                    content.Headers.TryAddWithoutValidation("Content-Type", "multipart/related; boundary=HiEIS");
                    */
                    
                    var response = await httpClient.PostAsync(url, content);
                    var result = Newtonsoft.Json.JsonConvert.DeserializeObject<GoogleDriveUploadFileSuccessVM>(await response.Content.ReadAsStringAsync());
                    if (result.Id == null) return BadRequest("Upload Failed!");

                    invoice.GoogleDriveFileId = result.Id;
                    _invoiceService.UpdateInvoice(invoice);
                    _invoiceService.SaveChanges();

                    return Ok();
                }

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
