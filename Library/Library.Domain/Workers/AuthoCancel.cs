using Library.Database.Entities;
using Library.Database.Interfaces;
using Quartz;

namespace Library.Domain.Workers
{
    [DisallowConcurrentExecution]
    public class AuthoCancel : IAuthoCancel
    {
        private readonly IOrderRepository<Order> orderRepository;
        private readonly IBookRepository<Book> bookRepository;

        public AuthoCancel(IOrderRepository<Order> orderRepository, IBookRepository<Book> bookRepository)
        {
            this.orderRepository = orderRepository;
            this.bookRepository = bookRepository;
        }

        public void CancelAutho(Order order)
        {
            var book = bookRepository.GetBook(order.BookId);
            orderRepository.DeleteOrder(order, book);
        }
    }
}
