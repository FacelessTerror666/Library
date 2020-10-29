using Library.Database.Entities;
using Library.Database.Enums;
using Library.Database.Interfaces;
using Library.Domain.Interfaces;
using Library.Domain.Models.Orders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Domain.Services
{
    public class ReportService : IReportService
    {
        private readonly IRepository<Book> bookRepository;
        private readonly IRepository<Order> orderRepository;

        public ReportService(IRepository<Book> bookRepository, IRepository<Order> orderRepository)
        {
            this.bookRepository = bookRepository;
            this.orderRepository = orderRepository;
        }
  
        public async Task<OrdersListModel> ReportSearchAsync(string orderStatus, int timeReport)
        {
            var orders = orderRepository.GetItems();

            if (orderStatus == "Booked")
            {
                orders = orders.Where(x => x.OrderStatus == OrderStatus.Booked);
            }
            if (orderStatus == "Given")
            {
                orders = orders.Where(x => x.OrderStatus == OrderStatus.Given);
            }
            if (orderStatus == "Cancelled")
            {
                orders = orders.Where(x => x.OrderStatus == OrderStatus.Cancelled);
            }
            if (orderStatus == "Returned")
            {
                orders = orders.Where(x => x.OrderStatus == OrderStatus.Returned);
            }

            if (timeReport == 0) // За день.
            {
                orders = orders.Where(x => x.DateBooking.AddDays(1) >= DateTime.Now);
            }
            if (timeReport == 10) // За неделю.
            {
                orders = orders.Where(x => x.DateBooking.AddDays(7) >= DateTime.Now);
            }
            if (timeReport == 20) // За месяц.
            {
                orders = orders.Where(x => x.DateBooking.AddMonths(1) >= DateTime.Now);
            }

            var orderSearchVM = new OrdersListModel
            {
                Orders = await orders.Include(x => x.Book).Include(x => x.User).ToListAsync()
            };

            return orderSearchVM;
        }
    }
}
