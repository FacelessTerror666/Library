using System.ComponentModel.DataAnnotations;

namespace Library.Database.Enums
{
    public enum BookStatus
    {
        /// <summary>
        /// Свободна
        /// </summary>
        [Display(Name = "Свободна")]
        Free = 0,

        /// <summary>
        /// Забронирована
        /// </summary>
        [Display(Name = "Забронирована")]
        Booked = 10,

        /// <summary>
        /// Выдана
        /// </summary>
        [Display(Name = "Выдана")]
        Given = 20
    }
}
