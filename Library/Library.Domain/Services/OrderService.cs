using Library.Database;
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

            DateTime now = DateTime.Now;

            var order = new Order
            {
                BookId = book.Id,
                UserId = user.Id,
                User = user,
                DateBooking = now,
                DateReturned = now.AddDays(7),
                OrderStatus = OrderStatus.Booked
            };
            orderRepository.Create(order);

            if (book.BookStatus == BookStatus.Free)
            {
                book.BookStatus = BookStatus.Booked;
            }
            bookRepository.Update(book);
        }

        public IEnumerable<ReaderOrdersModel> ReaderOrders(long userId)
        {
            var orders = orderRepository.GetItems()
                .Include(x => x.Book)
                .Where(x => x.UserId == userId)
                .Where(x => x.OrderStatus == OrderStatus.Booked || x.OrderStatus == OrderStatus.Given)
                .Where(x => x.Book.IsDeleted == false)
                .OrderByExp("DateBooking")
                .Select(x => new ReaderOrdersModel
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    User = x.User,
                    BookId = x.BookId,
                    Book = x.Book,
                    DateReturned = x.DateReturned,
                })
                .ToList();

            return orders;
        }

        public IEnumerable<AllOrdersModel> AllOrders()
        {
            var anyOrders = orderRepository.GetItems()
                .Include(x => x.Book)
                .Where(x => x.OrderStatus == OrderStatus.Booked || x.OrderStatus == OrderStatus.Given)
                .Where(x => x.Book.IsDeleted == false)
                .OrderByExp("DateBooking")
                .Select(x => new AllOrdersModel
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    User = x.User,
                    BookId = x.BookId,
                    Book = x.Book,
                    DateReturned = x.DateReturned,
                })
                .ToList();

            return anyOrders;
        }

        public void GiveOutBook(long id)
        {
            var order = orderRepository.Get(id);
            var bookId = order.BookId;
            var book = bookRepository.Get(bookId);
            if (book.BookStatus == BookStatus.Booked)
            {
                order.OrderStatus = OrderStatus.Given;
            }
            book.BookStatus = BookStatus.Given;
            bookRepository.Update(book);
            orderRepository.Update(order);
        }

        public void ReturnBook(long id)
        {
            var order = orderRepository.Get(id);
            var bookId = order.BookId;
            var book = bookRepository.Get(bookId);
            if (book.BookStatus == BookStatus.Given)
            {
                order.OrderStatus = OrderStatus.Returned;
                order.DateReturned = DateTime.Now;
            }
            book.BookStatus = BookStatus.Free;
            bookRepository.Update(book);
            orderRepository.Update(order);
        }

        public void CancelReservation(long id)
        {
            var order = orderRepository.Get(id);
            var bookId = order.BookId;
            var book = bookRepository.Get(bookId);
            if (book.BookStatus == BookStatus.Booked)
            {
                order.OrderStatus = OrderStatus.Cancelled;
                order.DateReturned = DateTime.Now;
            }
            book.BookStatus = BookStatus.Free;
            bookRepository.Update(book);
            orderRepository.Update(order);
        }

        public void AuthoCancelReservation(Order order)
        {
            var bookId = order.BookId;
            var book = bookRepository.Get(bookId);
            if (book.BookStatus == BookStatus.Booked)
            {
                order.OrderStatus = OrderStatus.Cancelled;
                order.DateReturned = DateTime.Now;
            }
            book.BookStatus = BookStatus.Free;
            bookRepository.Update(book);
            orderRepository.Update(order);
        }
    }
}
