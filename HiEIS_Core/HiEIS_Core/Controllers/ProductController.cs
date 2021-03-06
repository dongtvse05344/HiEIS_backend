﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HiEIS.Model;
using HiEIS.Service;
using HiEIS_Core.Paging;
using HiEIS_Core.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Authorization;
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
        public ActionResult GetProducts(int index = 1, int pageSize = 5, string nameSearch = "",string codeSearch = "")
        {
            try
            {
                var user = _userManager.GetUserAsync(User).Result;

                codeSearch = codeSearch == null ? "" : codeSearch;
                nameSearch = nameSearch == null ? "" : nameSearch;
                var products = _productService
                    .GetProducts(_ => _.CompanyId.Equals(user.Staff.CompanyId) && 
                                    _.Name.ToLower().Contains(nameSearch.ToLower()) &&
                                    _.Code.ToLower().Contains(codeSearch.ToLower())
                                    );

                //Paging
                var result = products.ToPageList<ProductVM, Product>(index, pageSize);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [Authorize]
        [HttpGet("GetAll")]
        public ActionResult GetProducts()
        {
            try
            {
                var user = _userManager.GetUserAsync(User).Result;
                var products = _productService.GetProducts(
                    _ => _.IsActive == true && _.CompanyId.Equals(user.Staff.CompanyId))
                    .OrderBy(_ => _.Name).ToList();
                List<ProductVM> result = new List<ProductVM>();
                foreach (var item in products)
                {
                    result.Add(item.Adapt<ProductVM>());
                }
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
                if (product == null) return NotFound();
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
                if (product == null) return NotFound();
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
                return StatusCode(201, product.Id);
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