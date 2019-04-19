using HiEIS.Model;
using HiEIS.Service;
using HiEIS_Core.Paging;
using HiEIS_Core.Utils;
using HiEIS_Core.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NSoup;
using NSoup.Nodes;
using NSoup.Parse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiEIS_Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public ActionResult GetCustomers(int index = 1, int pageSize = 5, string nameSearch = "")
        {
            nameSearch = nameSearch == null ? "" : nameSearch;

            var customers = _customerService.GetCustomers(_ => _.Enterprise.ToLower().Contains(nameSearch.ToLower()));
            var result = customers.ToPageList<CustomerVM, Customer>(index, pageSize);

            return Ok(result);
        }

        [HttpPost]
        public ActionResult CreateCustomer([FromBody]CustomerCM model)
        {
            try
            {
                var customer = model.Adapt<Customer>();
                _customerService.CreateCustomer(customer);
                _customerService.SaveChanges();
                return StatusCode(201, customer.Id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        public ActionResult DeleteCustomer(Guid id)
        {
            try
            {
                var customer = _customerService.GetCustomer(id);
                if (customer == null) return NotFound();

                _customerService.DeleteCustomer(customer);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public ActionResult UpdateCustomer([FromBody]CustomerUM model)
        {
            try
            {
                var customer = _customerService.GetCustomer(model.id);
                if (customer == null) return NotFound();

                customer = model.Adapt(customer);
                _customerService.UpdateCustomer(customer);
                _customerService.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        private CompanyVM GetDataFromOutSide(string taxNo)
        {
            string url = "http://www.thongtincongty.com/search/";
            IConnection connection = NSoupClient.Connect(url += taxNo);
            Document document = connection.Get();

            string html = document.GetElementsByClass("jumbotron").OuterHtml();
            document = Parser.Parse(html, document.BaseUri);
            string[] arr = html.Split("<br />");

            var companyVM = new CompanyVM();
            companyVM.Enterprise = document.Select("span").Text;
            foreach (var item in arr)
            {
                if (item.Contains("Địa chỉ"))
                {
                    var address = item.Substring(item.IndexOf("Địa") + 9);
                    companyVM.Address = StringUtils.Replace(address.Substring(0, address.Length - 2));
                    break;
                }
            }
            return companyVM;
        }

        [HttpGet("GetEnterprise")]
        public ActionResult GetEnterpriseInfoByTaxNo(string taxNo)
        {
            var customer = _customerService.GetCustomers().FirstOrDefault(_ => _.TaxNo.Equals(taxNo));
            if(customer == null)
            {
                return Ok(this.GetDataFromOutSide(taxNo));
            }
            else
            {
                var result = customer.Adapt<CompanyVM>();
                result.Email = JsonConvert.DeserializeObject<string[]>(customer.Emails);
                return Ok(result);
            }
        }
    }
}
