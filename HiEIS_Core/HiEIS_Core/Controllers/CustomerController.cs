using HiEIS.Model;
using HiEIS.Service;
using HiEIS_Core.Paging;
using HiEIS_Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
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
        public ActionResult GetCustomers(int index = 1, int pageSize = 5, string searchName = "")
        {
            searchName = searchName == null ? "" : searchName;

            var customers = _customerService.GetCustomers(_ => _.Name.ToLower().Equals(searchName.ToLower()));
            var result = customers.ToPageList<CustomerVM, Customer>(index, pageSize);

            return Ok(result);
        }
    }
}
