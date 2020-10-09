using System.ComponentModel.DataAnnotations;

namespace Library.Domain.Models.Books
{
    public class BookModel
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
    }
}
