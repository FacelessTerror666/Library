using Library.Database.Entities;
using System;

namespace Library.Models.Orders
{
    public class AllOrdersListModel
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public User User { get; set; }

        public long BookId { get; set; }

        public Book Book { get; set; }
    }
}
