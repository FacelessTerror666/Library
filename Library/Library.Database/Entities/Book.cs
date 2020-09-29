using Library.Database.Enums;
using Library.Database.Interfaces;

namespace Library.Database.Entities
{
    public class Book : IEntity
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Author { get; set; }

        public string Genre { get; set; }

        public string Publisher { get; set; }

        public string Description { get; set; }

        public BookStatus BookStatus { get; set; }

        public bool IsDeleted { get; set; }
    }
}
