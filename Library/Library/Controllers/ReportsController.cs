using Library.Domain.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;
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
                var file_path = reportService.SaveReport(orderSearchVM.Orders);
                file_path = Path.Combine(appEnvironment.ContentRootPath, file_path);
                // Тип файла - content-type
                string file_type = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                // Имя файла - необязательно
                string file_name = "Отчёт.xlsx";
                return PhysicalFile(file_path, file_type, file_name);
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
