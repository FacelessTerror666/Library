using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library.Domain.Models.Books
{
    public class BooksListModel
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

        [Display(Name = "Статус")]
        public Database.Enums.BookStatus BookStatus { get; set; }

        public List<BooksListModel> Books { get; set; }

        public SelectList Authors { get; set; }

        public string BookAuthor { get; set; }

        public SelectList Genres { get; set; }

        public string BookGenre { get; set; }

        public SelectList Publishers { get; set; }

        public string BookPublisher { get; set; }

        public string SearchString { get; set; }
    }
}
