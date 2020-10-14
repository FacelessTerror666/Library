using Library.Database.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Library.Domain.Models.Orders
{
    public class ReaderOrdersListModel
    {
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Автор")]
        public string Author { get; set; }

        [Display(Name = "Жанр")]
        public string Genre { get; set; }

        [Display(Name = "Издательство")]
        public string Publisher { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Статус")]
        public Database.Enums.BookStatus BookStatus { get; set; }

        [Display(Name = "Дата")]
        public DateTime DateBooking { get; set; }

        public long Id { get; set; }

        public long UserId { get; set; }

        public User User { get; set; }

        public long BookId { get; set; }

        public Book Book { get; set; }
    }
}
