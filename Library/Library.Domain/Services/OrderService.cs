using Library.Database.Entities;
using Library.Database.Enums;
using Library.Database.Interfaces;
using Library.Domain.Interfaces;
using Library.Domain.Models.Orders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.Domain.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Book> bookRepository;
        private readonly IRepository<Order> orderRepository;

        public OrderService(IRepository<Book> bookRepository,
            IRepository<Order> orderRepository)
        {
            this.bookRepository = bookRepository;
            this.orderRepository = orderRepository;
        }

        public void Reservation(long id, User user)
        {
            var book = bookRepository.Get(id);

            var order = new Order
            {
                BookId = book.Id,
                UserId = user.Id,
                User = user,
                DateBooking = DateTime.Now.AddDays(7)
            };

            orderRepository.Create(order);
            if (book.BookStatus == BookStatus.Free)
            {
                book.BookStatus = BookStatus.Booked;
            }
            bookRepository.Update(book);
        }

        public IEnumerable<ReaderOrdersListModel> ReaderOrders(long userId)
        {
            var orders = orderRepository.GetItems()
                .Include(x => x.Book)
                .Where(x => x.UserId == userId)
                .Where(x => x.Book.IsDeleted == false)
                .OrderBy(x => x.Book.Name)
                .Select(x => new ReaderOrdersListModel
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    User = x.User,
                    BookId = x.BookId,
                    Book = x.Book,
                    DateBooking = x.DateBooking
                })
                .ToList();

            return orders;
        }

        public IEnumerable<AllOrdersListModel> AllOrders()
        {
            var anyOrders = orderRepository.GetItems()
               .Include(x => x.Book)
                .Where(x => x.Book.IsDeleted == false)
                .OrderBy(x => x.Book.Name)
                .Select(x => new AllOrdersListModel
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    User = x.User,
                    BookId = x.BookId,
                    Book = x.Book,
                    DateBooking = x.DateBooking
                })
                .ToList();

            return anyOrders;
        }

        public void GiveOutBook(long id)
        {
            var order = orderRepository.Get(id);
            var bookId = order.BookId;
            var book = bookRepository.Get(bookId);
            book.BookStatus = BookStatus.Given;
            orderRepository.Update(order);
        }

        public void ReturnBook(long id)
        {
            var order = orderRepository.Get(id);
            var bookId = order.BookId;
            var book = bookRepository.Get(bookId);
            book.BookStatus = BookStatus.Free;
            orderRepository.Delete(order);
        }

        public void CancelReservation(long id)
        {
            var order = orderRepository.Get(id);
            var bookId = order.BookId;
            var book = bookRepository.Get(bookId);
            book.BookStatus = BookStatus.Free;
            orderRepository.Delete(order);
        }

    }
}
