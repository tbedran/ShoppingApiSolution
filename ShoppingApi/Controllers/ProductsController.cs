using Microsoft.AspNetCore.Mvc;
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
        private readonly IProductCommands _productCommands;


        public ProductsController(ILookupProducts productLookup, IProductCommands productCommands)
        {
            _productLookup = productLookup;
            _productCommands = productCommands;
        }

        [HttpPost("/products")]
        public async Task<ActionResult> AddAProduct([FromBody] PostProductRequest productToAdd)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                
                //Add it to the domain
                GetProductDetailsResponse response = await _productCommands.Add(productToAdd);
                //return :
                //201 Created
                //Location header with the url of the ew thingy
                //and a copy of what they would get if they followed that url

                //return Ok(productToAdd);

                return CreatedAtRoute("#products#getproductbyid", new { id = response.Id }, response);
            }
        }

        [HttpGet("/products/{id:int}", Name ="#products#getproductbyid")]
        public async Task<ActionResult> GetProductById(int id)
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
