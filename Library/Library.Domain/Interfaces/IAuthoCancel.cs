using Library.Database.Entities;

namespace Library.Domain.Interfaces
{
    public interface IAuthoCancel
    {
        void CancelAutho(Order order);
    }
}
