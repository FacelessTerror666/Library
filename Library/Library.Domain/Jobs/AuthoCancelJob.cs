using Library.Database.Entities;
using Library.Database.Enums;
using Library.Database.Interfaces;
using Library.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Domain.Jobs
{
    [DisallowConcurrentExecution]
    public class AuthoCancelJob : IJob
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        //private static readonly object _locker = new object();

        public AuthoCancelJob(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            //lock (_locker)
            //{
                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var authoCancel = scope.ServiceProvider.GetRequiredService<IOrderService>();
                    var orderRepository = scope.ServiceProvider.GetRequiredService<IRepository<Order>>();

                    var now = DateTime.Now;
                    var orders = orderRepository.GetItems()
                        .Where(x => x.DateReturned <= now)
                        .Where(x => x.OrderStatus == OrderStatus.Booked || x.OrderStatus == OrderStatus.Given)
                        .Where(x => x.Book.BookStatus == BookStatus.Booked || x.Book.BookStatus == BookStatus.Given)
                        .ToList();

                    foreach (var order in orders)
                    {
                        authoCancel.AuthoCancelReservation(order);
                    }

                    await Task.CompletedTask;
                }
            //}
        }
    }
}
