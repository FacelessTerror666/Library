﻿using System.ComponentModel.DataAnnotations;

namespace Library.Models.Orders
{
    public class ReaderOrdersListModel
    {
        public long Id { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Автор")]
        public string Author { get; set; }

        [Display(Name = "Жанр")]
        public string Genre { get; set; }

        [Display(Name = "Издательство")]
        public string Publisher { get; set; }
    }
}