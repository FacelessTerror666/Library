using Library.Database.Interfaces;
using System;

namespace Library.Database.Entities
{
    public class Order : IEntity
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public User User { get; set; }

        public long BookId { get; set; }

        public Book Book { get; set; }

        public DateTime DateBooking { get; set; }
    }
}
