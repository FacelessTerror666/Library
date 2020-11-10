using System.Linq;
using System.Threading.Tasks;

namespace Library.Database.Interfaces
{
    public interface IRepository<TEntity>
        where TEntity : class, IEntity
    {
        void Create(TEntity entity);

        Task CreateAsync(TEntity entity);

        public TEntity Get();

        void Update(TEntity entity);

        void Delete(TEntity entity);

        //void Delete(IEnumerable<TEntity> entities);

        IQueryable<TEntity> GetItems();

        TEntity Get(long id);
    }
}
