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
        public ActionResult GetAll(int index = 1, int pageSize = 5)
        {
            var list = _companyService.GetCompanys();
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
        }
    }
}