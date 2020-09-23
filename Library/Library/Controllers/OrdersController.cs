using Library.Database.Entities;
using Library.Database.Enums;
using Library.Database.Interfaces;
using Library.Models.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IRepository<Book> bookRepository;
        private readonly IRepository<Order> orderRepository;
        private readonly UserManager<User> userManager;

        public OrdersController(IRepository<Book> bookRepository, IRepository<Order> orderRepository, UserManager<User> userManager)
        {
            this.bookRepository = bookRepository;
            this.orderRepository = orderRepository;
            this.userManager = userManager;
        }

        [HttpGet]
        [Authorize(Roles = RoleInitialize.Reader)]
        public async Task<IActionResult> Booking(long id)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);

            var existingBook = bookRepository.Get(id);
            if (existingBook.BookStatus == BookStatus.Free)
            {
                existingBook.BookStatus = BookStatus.Booked;
            }
            bookRepository.Update(existingBook);

            var order = new Order { BookId = existingBook.Id, UserId = user.Id, User = user };
            orderRepository.Create(order);

            return RedirectToAction(nameof(ReaderOrders));
        }

        [HttpGet]
        [Authorize(Roles = RoleInitialize.Reader)]
        public async Task<IActionResult> ReaderOrders()
        {
            var user = await userManager.GetUserAsync(User);
            var userId = user.Id;

            var orders = await orderRepository.GetItems()
                .Include(x => x.Book)
                .Where(x => x.Book.IsDeleted == false)
                .Where(x => x.UserId == userId)
                .Select(x => new ReaderOrdersListModel
                {
                    Id = x.Book.Id,
                    Name = x.Book.Name,
                    Author = x.Book.Author,
                    Genre = x.Book.Genre,
                    Publisher = x.Book.Publisher,
                })
                .ToListAsync();

            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> AllOrders()
        {
            var anyOrders = await orderRepository.GetItems()
                .Include(x => x.Book)
                .Include(x => x.User)
                .Where(x => x.Book.IsDeleted == false)
                .Where(x => (x.Book.BookStatus == BookStatus.Booked || x.Book.BookStatus == BookStatus.Given))
                .Select(x => new AllOrdersListModel
                {
                    Id = x.Id,
                    Name = x.Book.Name,
                    Author = x.Book.Author,
                    Genre = x.Book.Genre,
                    Publisher = x.Book.Publisher,
                    BookStatus = x.Book.BookStatus
                })
                .ToListAsync();

            return View(anyOrders);
        }

        [HttpGet]
        public ActionResult ReturnBook(long id)
        {
            var order = orderRepository.GetItems()
                .FirstOrDefault(x => x.Id == id);

            var bookId = order.BookId;

            Book book = bookRepository.GetItems()
                .FirstOrDefault(x => x.Id == bookId);

            book.BookStatus = BookStatus.Free;

            orderRepository.Update(order);

            return RedirectToAction(nameof(AllOrders));
        }
    }
}
