﻿using Microsoft.AspNetCore.Mvc;
using ShoppingApi.Models.Products;
using ShoppingApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi.Controllers
{
    public class ProductsController: ControllerBase
    {
        private readonly ILookupProducts _productLookup;

        public ProductsController(ILookupProducts productLookup)
        {
            _productLookup = productLookup;
        }

        [HttpGet("/products/{id:int}")]
        public async Task<ActionResult> GetProductByIt(int id)
        {
            GetProductDetailsResponse response = await _productLookup.GetProductById(id);
            //if (response == null)
            //{
            //    return NotFound();
            //}
            //else
            //{
            //    return Ok(response);
            //}
            return this.Maybe(response);
        }
    }
}
