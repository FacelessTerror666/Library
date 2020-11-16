using Library.Domain.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Library.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IReportService reportService;
        private readonly IWebHostEnvironment appEnvironment;

        public ReportsController(IReportService reportService, IWebHostEnvironment appEnvironment)
        {
            this.reportService = reportService;
            this.appEnvironment = appEnvironment;
        }

        public async Task<IActionResult> ReportIndex(string orderStatus, int timeReport, bool save = false)
        {
            var orderSearchVM = await reportService.ReportSearchAsync(orderStatus, timeReport);

            if (save == true)
            {
                var memoryStream = reportService.SaveReport(orderSearchVM.Orders);
                string file_type = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                string file_name = "Отчёт.xlsx";
                return File(memoryStream, file_type, file_name);
            }

            return View(orderSearchVM);
        }

        public IActionResult Save()
        {
            ViewBag.Message = "Отчёт скачан";
            return View();
        }
    }
}
