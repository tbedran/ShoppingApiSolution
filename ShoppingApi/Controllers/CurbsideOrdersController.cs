﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingApi.Data;
using ShoppingApi.Models.CursideOrders;
using ShoppingApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi.Controllers
{
    public class CurbsideOrdersController : ControllerBase
    {

        private readonly ShoppingDataContext _context;
        private readonly CurbsideChannel _channel;

        public CurbsideOrdersController(ShoppingDataContext context, CurbsideChannel channel)
        {
            _context = context;
            _channel = channel;
        }

        [HttpPost("async/curbsideorders")]
        public async Task<ActionResult> AsyncCurbsideOrders([FromBody] PostSyncCurbsideOrdersRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                var orderToSave = new CurbsideOrder
                {
                    For = request.For,
                    Items = request.Items,
                };
                
                _context.CurbsideOrders.Add(orderToSave);
                await _context.SaveChangesAsync();

                var response = new GetCurbsideOrderResponse
                {
                    Id = orderToSave.Id,
                    For = orderToSave.For,
                    Items = orderToSave.Items,
                    //PickupDate = orderToSave.PickupDate.Value,
                    PickupDate = null,
                    Status = orderToSave.Status
                };
                await _channel.AddCurbside(new CurbsideChannelRequest { ReservationId = response.Id });
                return CreatedAtRoute("curbsideorders#getbyid", new { id = response.Id }, response);
                
            }
        }

        [HttpGet("curbsideorders/{id:int}", Name = "curbsideorders#getbyid")]
        public async Task<ActionResult> GetById(int id)
        {
            var order = await _context.CurbsideOrders
                .Select(order => new GetCurbsideOrderResponse
                {
                    Id = order.Id,
                    For = order.For,
                    Items = order.Items,
                    PickupDate = order.PickupDate.Value,
                    Status = order.Status
                }).SingleOrDefaultAsync(order => order.Id == id);
            return this.Maybe(order);
        }
            

        [HttpPost("sync/curbsideorders")]
        public async Task<ActionResult> SyncCurbsideOrders([FromBody] PostSyncCurbsideOrdersRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                var orderToSave = new CurbsideOrder
                {
                    For = request.For,
                    Items = request.Items,
                };
                var numberOfItems = orderToSave.Items.Split(',').Count();
                for (var t = 0; t < numberOfItems; t++)
                {
                    await Task.Delay(1000);
                }
                orderToSave.PickupDate = DateTime.Now.AddHours(numberOfItems);
                _context.CurbsideOrders.Add(orderToSave);
                await _context.SaveChangesAsync();

                var response = new GetCurbsideOrderResponse
                {
                    Id = orderToSave.Id,
                    For = orderToSave.For,
                    Items = orderToSave.Items,
                    PickupDate = orderToSave.PickupDate.Value
                };
                return Ok(response);
            }
            
           
        }
    }
}
