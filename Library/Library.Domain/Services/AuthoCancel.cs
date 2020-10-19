using Library.Database.Entities;
using Library.Database.Interfaces;
using Quartz;
using Library.Database.Enums;
using Library.Domain.Interfaces;

namespace Library.Domain.Services
{
    [DisallowConcurrentExecution]
    public class AuthoCancel : IAuthoCancel
    {
        private readonly IRepository<Order> orderRepository;
        private readonly IRepository<Book> bookRepository;

        public AuthoCancel(IRepository<Order> orderRepository, IRepository<Book> bookRepository)
        {
            this.orderRepository = orderRepository;
            this.bookRepository = bookRepository;
        }

        public void CancelAutho(Order order)
        {
            var book = bookRepository.Get(order.BookId);
            orderRepository.Delete(order);
            book.BookStatus = BookStatus.Free;
            bookRepository.Update(book);
        }
    }
}
