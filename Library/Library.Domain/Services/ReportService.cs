using Library.Database.Entities;
using Library.Database.Enums;
using Library.Database.Interfaces;
using Library.Domain.Interfaces;
using Library.Domain.Models.Orders;
using Microsoft.EntityFrameworkCore;
using NPOI.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Domain.Services
{
    public class ReportService : IReportService
    {
        private readonly IRepository<Order> orderRepository;

        public ReportService(IRepository<Order> orderRepository)
        {
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

        public MemoryStream SaveReport(List<Order> orders)
        {
            //Рабочая книга Excel
            XSSFWorkbook wb;
            //Лист в книге Excel
            XSSFSheet sh;

            //Создаем рабочую книгу
            wb = new XSSFWorkbook();
            //Создаём лист в книге
            sh = (XSSFSheet)wb.CreateSheet("Лист 1");

            //Заполняемые строки
            var rows = new List<string> {
            "Название книги","Имя клиента","Статус","Дата бронирования",
            "Дата снятия бронирования"
            };

            var i = 1;
            var j = 0;

            //Создаем строку c заголовками
            var currentRow = sh.CreateRow(0);
            foreach (var row in rows)
            {
                //в строке создаём ячеёку с указанием столбца
                var currentCell = currentRow.CreateCell(j);
                //в ячейку запишем заголовок
                currentCell.SetCellValue(row);
                //Выравним размер столбца по содержимому
                sh.AutoSizeColumn(j);
                j++;
            }

            //Запускаем цикл по строке
            foreach (var order in orders)
            {
                //Создаем строку
                var currentRoww = sh.CreateRow(i);

                var currentCell = currentRoww.CreateCell(0);//в строке создаём ячеqку с указанием столбца
                currentCell.SetCellValue(order.Book.Name);//в ячейку запишем информацию

                currentCell = currentRoww.CreateCell(1);
                currentCell.SetCellValue(order.User.UserName);

                currentCell = currentRoww.CreateCell(2);
                switch (order.OrderStatus)
                {
                    case OrderStatus.Booked:
                        currentCell.SetCellValue("Забронирован");
                        break;
                    case OrderStatus.Given:
                        currentCell.SetCellValue("Выдан");
                        break;
                    case OrderStatus.Cancelled:
                        currentCell.SetCellValue("Отменён");
                        break;
                    case OrderStatus.Returned:
                        currentCell.SetCellValue("Возвращен");
                        break;
                }

                currentCell = currentRoww.CreateCell(3);
                currentCell.SetCellValue(order.DateBooking.ToString());

                currentCell = currentRoww.CreateCell(4);
                currentCell.SetCellValue(order.DateReturned.ToString());

                //Выравним размер столбца по содержимому
                sh.AutoSizeColumn(j);
                i++;
            }

            ByteArrayOutputStream baos = new ByteArrayOutputStream();
            try
            {
                wb.Write(baos);
            }
            finally
            {
                baos.Close();
            }
            byte[] bytes = baos.ToByteArray();
            var memoryStream = new MemoryStream(bytes);

            return memoryStream;
        }
    }
}
