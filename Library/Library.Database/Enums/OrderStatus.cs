using System.ComponentModel.DataAnnotations;

namespace Library.Database.Enums
{
    public enum OrderStatus
    {
        /// <summary>
        /// Забронирован
        /// </summary>
        [Display(Name = "Забронирован")]
        Booked = 0,

        /// <summary>
        /// Выдан
        /// </summary>
        [Display(Name = "Выдан")]
        Given = 10,

        /// <summary>
        /// Отменён
        /// </summary>
        [Display(Name = "Отменён")]
        Cancelled = 20,

        /// <summary>
        /// Возвращён
        /// </summary>
        [Display(Name = "Возвращён")]
        Returned = 30
    }
}
