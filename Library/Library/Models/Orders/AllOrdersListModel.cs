using System.ComponentModel.DataAnnotations;

namespace Library.Models.Orders
{
    public class AllOrdersListModel
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
    }
}
