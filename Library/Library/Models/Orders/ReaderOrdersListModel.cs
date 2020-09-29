using Library.Database.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Library.Models.Orders
{
    public class ReaderOrdersListModel
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public User User { get; set; }

        public long BookId { get; set; }

        public Book Book { get; set; }
    }
}
