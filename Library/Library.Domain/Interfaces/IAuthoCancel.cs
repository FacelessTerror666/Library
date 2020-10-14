using Library.Database.Entities;

namespace Library.Domain.Workers
{
    public interface IAuthoCancel
    {
        void CancelAutho(Order order);
    }
}
