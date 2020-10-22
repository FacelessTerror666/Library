using Library.Database.Entities;
using Library.Domain.Models.Orders;
using System.Collections.Generic;

namespace Library.Domain.Interfaces
{
    public interface IOrderService
    {
        public void Reservation(long id, User user);

        public IEnumerable<ReaderOrdersListModel> ReaderOrders(long userId);

        public IEnumerable<AllOrdersListModel> AllOrders();

        public void GiveOutBook(long id);

        public void ReturnBook(long id);

        public void CancelReservation(long id);

        public void CancelReservation(Order order);
    }
}
