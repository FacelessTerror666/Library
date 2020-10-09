using Library.Database.Entities;
using Library.Database.Enums;
using Library.Database.Interfaces;
using Library.Domain.Models.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IBookRepository<Book> bookRepository;
        private readonly IOrderRepository<Order> orderRepository;
        private readonly UserManager<User> userManager;

        public OrdersController(IBookRepository<Book> bookRepository,
            IOrderRepository<Order> orderRepository,
            UserManager<User> userManager)
        {
            this.bookRepository = bookRepository;
            this.orderRepository = orderRepository;
            this.userManager = userManager;
        }

        [HttpGet]
        [Authorize(Roles = RoleInitialize.Reader)]
        public async Task<IActionResult> Reservation(long id)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);

            var book = bookRepository.GetBook(id);
            if (book.BookStatus == BookStatus.Free)
            {
                book.BookStatus = BookStatus.Booked;
            }
            bookRepository.UpdateBook(book);

            var order = new Order
            {
                BookId = book.Id,
                UserId = user.Id,
                User = user,
                DateBooking = DateTime.Now.AddDays(7)
            };
            orderRepository.CreateOrder(order, book);


            return RedirectToAction(nameof(ReaderOrders));
        }

        [HttpGet]
        [Authorize(Roles = RoleInitialize.Reader)]
        public async Task<IActionResult> ReaderOrders()
        {
            var user = await userManager.GetUserAsync(User);
            var userId = user.Id;

            var orders = await orderRepository.GetOrders()
                .Include(x => x.Book)
                .Include(x => x.User)
                .Where(x => x.Book.IsDeleted == false)
                .Where(x => x.UserId == userId)
                .Select(x => new ReaderOrdersListModel
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    User = x.User,
                    BookId = x.BookId,
                    Book = x.Book,
                    DateBooking = x.DateBooking
                })
                .ToListAsync();

            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> AllOrders()
        {
            var anyOrders = await orderRepository.GetOrders()
                .Include(x => x.Book)
                .Include(x => x.User)
                .Where(x => x.Book.IsDeleted == false)
                .Select(x => new AllOrdersListModel
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    User = x.User,
                    BookId = x.BookId,
                    Book = x.Book,
                    DateBooking = x.DateBooking
                })
                .ToListAsync();

            return View(anyOrders);
        }

        [HttpGet]
        public ActionResult GiveOutBook(long id)
        {
            var order = orderRepository.GetOrder(id);
            var bookId = order.BookId;
            var book = bookRepository.GetBook(bookId);
            book.BookStatus = BookStatus.Given;
            orderRepository.UpdateOrder(order);

            return RedirectToAction(nameof(AllOrders));
        }

        [HttpGet]
        public ActionResult ReturnBook(long id)
        {
            var order = orderRepository.GetOrder(id);
            var bookId = order.BookId;
            var book = bookRepository.GetBook(bookId);
            book.BookStatus = BookStatus.Free;
            orderRepository.DeleteOrder(order, book);

            return RedirectToAction(nameof(AllOrders));
        }

        [HttpGet]
        public ActionResult CancelReservation(long id)
        {
            var order = orderRepository.GetOrder(id);
            var bookId = order.BookId;
            var book = bookRepository.GetBook(bookId);
            book.BookStatus = BookStatus.Free;
            orderRepository.DeleteOrder(order, book);

            return RedirectToAction(nameof(ReaderOrders));
        }

    }
}