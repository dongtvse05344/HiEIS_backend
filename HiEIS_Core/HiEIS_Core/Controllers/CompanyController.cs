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
using HiEIS_Core.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Http;
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

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
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
        public async Task<ActionResult> GetEnterpriseInfoByTaxNo(string taxNo)
        {
            string url = "http://www.thongtincongty.com/search/";
            IConnection connection = NSoupClient.Connect(url += "taxNo");
            Document document = connection.Get();

            string html = document.GetElementsByClass("jumbotron").OuterHtml();
            document = Parser.Parse(html, document.BaseUri);
            string[] arr = html.Split("<br />");

            var company = _companyService.GetCompanys(_ => _.TaxNo.Equals(taxNo));
            var companyVM = company.Adapt<CompanyVM>();

            companyVM.Tel = document.Select("img")[1].Attr("src");
            companyVM.Name = document.Select("span").Text;
            companyVM.ActiveType = arr[0].Substring(arr[0].IndexOf("Loại"));
            companyVM.Address = arr[2].Substring(arr[2].IndexOf("Địa"));
            companyVM.LegalRepresentative = arr[3].Substring(arr[3].IndexOf("Đại"));
            companyVM.LicenseDate = arr[4].Substring(arr[4].IndexOf("Ng"));
            companyVM.ActiveDate = arr[5].Substring(arr[5].IndexOf("Ng"), arr[5].IndexOf("2017") + 4)
                                    + "(" + document.Select("em").Text + ")";

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