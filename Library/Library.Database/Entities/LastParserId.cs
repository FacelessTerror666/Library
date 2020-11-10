using Library.Database.Interfaces;

namespace Library.Database.Entities
{
    public class LastParserId : IEntity
    {
        public long Id { get; set; }

        public long LastId { get; set; }
    }
}
