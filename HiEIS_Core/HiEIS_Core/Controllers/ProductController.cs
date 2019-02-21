using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HiEIS.Model;
using HiEIS.Service;
using HiEIS_Core.Paging;
using HiEIS_Core.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HiEIS_Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        [HttpGet]
        public ActionResult GetProducts(int index = 1, int pageSize =5 , string nameSearch ="")
        {
            nameSearch = nameSearch == null ? "" : nameSearch;

            var product = _productService.GetProducts();
            //Search
            product = product.Where(_ => _.Name.Contains(nameSearch));

            //Paging

            var result = product.ToPageList<ProductVM, Product>(index, pageSize);
            return Ok(result);
        }

        public ActionResult Get(Guid id)
        {
            var product = _productService.GetProduct(Guid.NewGuid());
            return Ok(product.Adapt<ProductVM>());
        }

        public ActionResult Update(ProductUM model)
        {
            var product = _productService.GetProduct(model.Id);
            product = model.Adapt(product);
            return Ok();
        }
    }
}