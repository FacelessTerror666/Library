﻿using Library.Database.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Database
{
    public class Repository<TEntity> : IRepository<TEntity>
         where TEntity : class, IEntity
    {
        private readonly LibraryDbContext _context;

        public Repository(LibraryDbContext context)
        {
            _context = context;
        }

        public void Create(TEntity entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
        }

        public async Task CreateAsync(TEntity entity)
        {
            _context.Add(entity);
            await _context.SaveChangesAsync();
        }

        public TEntity Get()
        {
            return _context.Set<TEntity>().FirstOrDefault();
        }

        public void Delete(TEntity entity)
        {
            if (entity != null)
            {
                _context.Remove(entity);
                _context.SaveChanges();
            }
        }

        //public void Delete(IEnumerable<TEntity> entities)
        //{
        //    if (entities.Count() > 0)
        //    {
        //        _context.RemoveRange(entities);
        //        _context.SaveChanges();
        //    }
        //}

        public IQueryable<TEntity> GetItems()
        {
            return _context.Set<TEntity>();
        }

        public void Update(TEntity entity)
        {
            _context.SaveChanges();
        }

        public TEntity Get(long id)
        {
            return _context.Set<TEntity>().Find(id);
        }
    }
}
