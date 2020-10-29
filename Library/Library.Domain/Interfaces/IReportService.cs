using Library.Domain.Models.Orders;
using System.Threading.Tasks;

namespace Library.Domain.Interfaces
{
    public interface IReportService
    {
        public Task<OrdersListModel> ReportSearchAsync(string orderStatus, int timeReport);
    }
}
