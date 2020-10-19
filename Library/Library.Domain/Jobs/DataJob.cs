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
    public class DataJob : IJob
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private static readonly object _locker = new object();

        public DataJob(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public Task Execute(IJobExecutionContext context)
        {
            lock (_locker)
            {
                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var authoCancel = scope.ServiceProvider.GetRequiredService<IAuthoCancel>();
                    var orderRepository = scope.ServiceProvider.GetRequiredService<IRepository<Order>>();

                    var now = DateTime.Now;
                    var orders = orderRepository.GetItems()
                        .Where(x => x.DateBooking <= now)
                        .Where(x => x.Book.BookStatus == BookStatus.Booked)
                        .ToList();

                    foreach (var order in orders)
                    {
                        authoCancel.CancelAutho(order);
                    }

                    return Task.CompletedTask;
                }
            }
        }
    }
}
