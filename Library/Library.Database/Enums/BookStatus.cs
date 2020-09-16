using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Database.Enums
{
    public enum BookStatus
    {
        /// <summary>
        /// Свободна
        /// </summary>
        Free = 0,

        /// <summary>
        /// Забронирована
        /// </summary>
        Booked = 10,

        /// <summary>
        /// Выдана
        /// </summary>
        Given = 20
    }
}
