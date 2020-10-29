using Library.Database.Entities;
using Library.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Library.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderService orderService;
        private readonly UserManager<User> userManager;

        public OrdersController(IOrderService orderService,
            UserManager<User> userManager)
        {
            this.orderService = orderService;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Reservation(long id)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            orderService.Reservation(id, user);
            return RedirectToAction(nameof(ReaderOrders));
        }

        [HttpGet]
        public async Task<IActionResult> ReaderOrders()
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            var userId = user.Id;
            var orders = orderService.ReaderOrders(userId);
            return View(orders);
        }

        [HttpGet]
        public IActionResult AllOrders()
        {
            var anyOrders = orderService.AllOrders();
            return View(anyOrders);
        }

        [HttpGet]
        public ActionResult GiveOutBook(long id)
        {
            orderService.GiveOutBook(id);
            return RedirectToAction(nameof(AllOrders));
        }

        [HttpGet]
        public ActionResult ReturnBook(long id)
        {
            orderService.ReturnBook(id);
            return RedirectToAction(nameof(AllOrders));
        }

        [HttpGet]
        public ActionResult CancelReservation(long id)
        {
            orderService.CancelReservation(id);
            return RedirectToAction(nameof(ReaderOrders));
        }
    }
}