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

        public async Task<IActionResult> ReportIndex(string orderStatus, int timeReport, bool save = false)
        {
            var orderSearchVM = await reportService.ReportSearchAsync(orderStatus, timeReport);

            if (save == true)
            {
                reportService.SaveReport(orderSearchVM.Orders);
            }

            return View(orderSearchVM);
        }

        public IActionResult Save()
        {
            ViewBag.Message = "Отчёт скачен";
            return View();
        }
    }
}
