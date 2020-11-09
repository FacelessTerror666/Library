using System.ComponentModel.DataAnnotations;

namespace Library.Domain.Models.Books
{
    public class EditBookModel
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

        [Display(Name = "Описание")]
        public string Description { get; set; }

        public string Img { get; set; }

        public string ImgPath { get; set; }
    }
}
