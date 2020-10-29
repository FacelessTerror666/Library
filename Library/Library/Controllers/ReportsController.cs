using Library.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Library.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IReportService reportService;

        public ReportsController(IReportService reportService)
        {
            this.reportService = reportService;
        }

        public async Task<IActionResult> ReportIndex(string orderStatus, int timeReport)
        {
            var orderSearchVM = await reportService.ReportSearchAsync(orderStatus, timeReport);

            return View(orderSearchVM);
        }
    }
}
