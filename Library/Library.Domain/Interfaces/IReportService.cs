using Library.Database.Entities;
using Library.Domain.Models.Orders;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Domain.Interfaces
{
    public interface IReportService
    {
        public Task<OrdersListModel> ReportSearchAsync(string orderStatus, int timeReport);

        public string SaveReport(List<Order> orders);
    }
}
