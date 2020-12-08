using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ShoppingApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingApi.Services
{
    public class CurbsideOrderProcessor : BackgroundService
    {
        private readonly ILogger<CurbsideOrderProcessor> _logger;
        private readonly CurbsideChannel _channel;
        private readonly IServiceProvider _serviceProvider;

        public CurbsideOrderProcessor(ILogger<CurbsideOrderProcessor> logger, CurbsideChannel channel, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _channel = channel;
            _serviceProvider = serviceProvider;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //while (true)
            //{
            //    await Task.Delay(1000);
            //    _logger.LogInformation("Background worked doing it thing");
            //    if (stoppingToken.IsCancellationRequested)
            //    {
            //        break;
            //    }
            //}

            await foreach(var order in _channel.ReadAllAsync())
            {
                using var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ShoppingDataContext>();

                var savedOrder = await context.CurbsideOrders.SingleOrDefaultAsync(o => o.Id == order.ReservationId);

                if (savedOrder == null)
                {
                    continue;
                }
                else
                {
                    var numberOfItems = savedOrder.Items.Split(',').Count();
                    for (var t = 0; t < numberOfItems; t++)
                    {
                        await Task.Delay(1000);
                    }
                    savedOrder.Status = CurbsideorderStatus.Approved;
                    savedOrder.PickupDate = DateTime.Now.AddHours(numberOfItems);
                    await context.SaveChangesAsync();
                }
                
            }

        }
    }
}
