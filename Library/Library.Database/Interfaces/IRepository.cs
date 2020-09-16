using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.Database.Interfaces
{
    public interface IRepository<TEntity>
        where TEntity : class, IEntity
    {
        void Create(TEntity entity);

        //void Update(TEntity entity);
        void Delete(TEntity entity);

        //void Delete(IEnumerable<TEntity> entities);

        IQueryable<TEntity> GetItems();

        TEntity Get(long id);
    }
}
