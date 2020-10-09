using Library.Database.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Library.Database.Interfaces
{
    public interface IOrderRepository<TEntity>
        where TEntity : class, IEntity
    {
        void CreateOrder(TEntity entity, Book book);

        void UpdateOrder(TEntity entity);

        void DeleteOrder(TEntity entity, Book book);

        void DeleteOrders(IEnumerable<TEntity> entities);

        IQueryable<TEntity> GetOrders();

        TEntity GetOrder(long id);
    }
}
