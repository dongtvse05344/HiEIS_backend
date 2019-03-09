using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using HiEIS.Model;
using HiEIS.Service;
using HiEIS_Core.Paging;
using HiEIS_Core.Utils;
using HiEIS_Core.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NSoup;
using NSoup.Nodes;
using NSoup.Parse;

namespace HiEIS_Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly UserManager<MyUser> _userManager;
        private readonly IStaffService _staffService;

        public CompanyController(ICompanyService companyService, UserManager<MyUser> userManager, IStaffService staffService)
        {
            _companyService = companyService;
            _userManager = userManager;
            _staffService = staffService;
        }

        [Authorize(Roles = "Manager")]
        [HttpGet("GetCompany")]
        public async Task<ActionResult> GetCompany()
        {
            var user = await _userManager.GetUserAsync(User);
            var staff = _staffService.GetStaff(user.Id);
            return Ok(staff.Company.Adapt<CompanyVM>());
        }

        [HttpGet]
        public ActionResult GetAll(string name, string taxNo, string address, string tel, int index = 1, int pageSize = 5)
        {
            name = name != null ? name : "";
            address = address != null ? address : "";
            tel = tel != null ? tel : "";
            taxNo = taxNo != null ? taxNo : "";

            var list = _companyService.GetCompanys();
            list = list.Where(_ => _.Name.Contains(name)
                                    && _.TaxNo.Contains(taxNo)
                                    && _.Address.Contains(address)
                                    && _.Tel.Contains(tel));
            var result = list.ToPageList<CompanyVM, Company>(index, pageSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult Get(Guid id)
        {
            var result = _companyService.GetCompany(id);
            return Ok(result.Adapt<CompanyVM>());
        }

        [HttpPut("{id}/ToggleActive")]
        public ActionResult ToggleActive(Guid id)
        {
            try
            {
                var company = _companyService.GetCompany(id);
                company.IsActive = !company.IsActive;
                _companyService.UpdateCompany(company);
                _companyService.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public ActionResult Post([FromBody] CompanyCM model)
        {
            try
            {
                var company = model.Adapt<Company>();
                _companyService.CreateCompany(company);
                _companyService.SaveChanges();
                return StatusCode(201);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public ActionResult Put([FromBody] CompanyUM model)
        {
            try
            {
                var company = _companyService.GetCompany(model.Id);
                if (company == null) return NotFound();
                company = model.Adapt(company);
                _companyService.UpdateCompany(company);
                _companyService.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(Guid id)
        {
            try
            {
                var company = _companyService.GetCompany(id);
                _companyService.DeleteCompany(company);
                _companyService.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetEnterprise")]
        public ActionResult GetEnterpriseInfoByTaxNo(string taxNo)
        {
            string url = "http://www.thongtincongty.com/search/";
            IConnection connection = NSoupClient.Connect(url += taxNo);
            Document document = connection.Get();

            string html = document.GetElementsByClass("jumbotron").OuterHtml();
            document = Parser.Parse(html, document.BaseUri);
            string[] arr = html.Split("<br />");

            //  var company = _companyService.GetCompanys(_ => _.TaxNo.Equals(taxNo)).FirstOrDefault();
            // var companyVM = company.Adapt<CompanyVM>();

            var companyVM = new CompanyVM();
            companyVM.Name = document.Select("span").Text;
            foreach (var item in arr)
            {
                if (item.Contains("Địa chỉ"))
                {
                    var address = item.Substring(item.IndexOf("Địa") + 9);
                    companyVM.Address = StringUtils.Replace(address.Substring(0, address.Length - 2));
                    break;
                }
            }
            
            return Ok(companyVM);

            /*
            using (HttpClient client = new HttpClient())
            {
                StringBuilder apiUrl = new StringBuilder("/api/company/");
                apiUrl.Append(taxNo);

                client.BaseAddress = new Uri("https://thongtindoanhnghiep.co");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(apiUrl.ToString());
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var info = JsonConvert.DeserializeObject<EnterpriseTaxVM>(data);
                    return Ok(info.Adapt<CompanyVM>());
                }
            }
            
            return NotFound();
            */
        }
    }
}