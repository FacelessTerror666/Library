﻿using Library.Database.Interfaces;

namespace Library.Database.Entities
{
    public class Order : IEntity
    {
        public long Id { get; set; }

        public User User { get; set; }

        public long BookId { get; set; }

        public Book Book { get; set; }
    }
}