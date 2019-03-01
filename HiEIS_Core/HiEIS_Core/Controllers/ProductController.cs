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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HiEIS_Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly UserManager<MyUser> _userManager;

        public ProductController(IProductService productService, UserManager<MyUser> userManager)
        {
            _productService = productService;
            _userManager = userManager;
        }

        [HttpGet]
        public ActionResult GetProducts(int index = 1, int pageSize = 5, string nameSearch = "")
        {
            try
            {
                var user = _userManager.GetUserAsync(User).Result;

                nameSearch = nameSearch == null ? "" : nameSearch;
                var products = _productService
                    .GetProducts(_ => _.CompanyId.Equals(user.Staff.CompanyId) && 
                                    _.Name.ToLower().Contains(nameSearch.ToLower()));

                //Paging
                var result = products.ToPageList<ProductVM, Product>(index, pageSize);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet("{id}")]
        public ActionResult Get(Guid id)
        {
            try
            {
                var product = _productService.GetProduct(id);
                return Ok(product.Adapt<ProductVM>());
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpPut]
        public ActionResult Update(ProductUM model)
        {
            try
            {
                var product = _productService.GetProduct(model.Id);
                product = model.Adapt(product);
                _productService.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost]
        public ActionResult Create([FromBody]ProductCM model)
        {
            try
            {
                var user = _userManager.GetUserAsync(User).Result;
                var product = model.Adapt<Product>();
                product.CompanyId = user.Staff.CompanyId;
                _productService.CreateProduct(product);
                _productService.SaveChanges();
                return StatusCode(201);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(Guid id)
        {
            try
            {
                var product = _productService.GetProduct(id);
                //product.IsActive = false;
                //_productService.UpdateProduct(product);
                _productService.DeleteProduct(product);
                _productService.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}