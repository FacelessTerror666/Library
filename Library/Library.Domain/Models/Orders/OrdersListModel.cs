using Library.Database.Entities;
using Library.Database.Enums;
using System.Collections.Generic;

namespace Library.Domain.Models.Orders
{
    public class OrdersListModel
    {
        public List<Order> Orders { get; set; }

        public string OrderStatus { get; set; }

        public TimeReport TimeReport { get; set; }
    }
}
