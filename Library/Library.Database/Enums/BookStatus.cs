using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Library.Database.Enums
{
    public enum BookStatus
    {
        /// <summary>
        /// Свободная
        /// </summary>
        [Display(Name = "Свободная")]
        Free = 0,

        /// <summary>
        /// Забронированная
        /// </summary>
        [Display(Name = "Забронированная")]
        Booked = 10,

        /// <summary>
        /// Выданная
        /// </summary>
        [Display(Name = "Выданная")]
        Given = 20
    }
}
