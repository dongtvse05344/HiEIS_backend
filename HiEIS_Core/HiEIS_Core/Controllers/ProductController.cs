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
        public ActionResult GetProducts(int index = 1, int pageSize = 5, string nameSearch = "")
        {
            nameSearch = nameSearch == null ? "" : nameSearch;

            var products = _productService
                .GetProducts(_ => _.Name.ToLower().Contains(nameSearch.ToLower()));

            //Paging
            var result = products.ToPageList<ProductVM, Product>(index, pageSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult Get(Guid id)
        {
            var product = _productService.GetProduct(id);
            return Ok(product.Adapt<ProductVM>());
        }

        [HttpPut]
        public ActionResult Update(ProductUM model)
        {
            var product = _productService.GetProduct(model.Id);
            product = model.Adapt(product);
            _productService.SaveChanges();
            return Ok();
        }

        [HttpPost]
        public ActionResult Create([FromBody]ProductCM model)
        {
            var product = model.Adapt(new Product());
            _productService.CreateProduct(product);
            _productService.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(Guid id)
        {
            var product = _productService.GetProduct(id);
            _productService.UpdateProduct(product);
            _productService.SaveChanges();
            return Ok();
        }
    }
}