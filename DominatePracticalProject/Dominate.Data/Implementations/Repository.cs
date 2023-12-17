using Dominate.Data.Context;
using Dominate.Data.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominate.Data.Implementations
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly UserDbContext _context;
        private readonly DbSet<T> _dbSet;
        public Repository(UserDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<EntityState> Add(T entity)
        {
            var entityState = await _dbSet.AddAsync(entity);
            return entityState.State;
        }
      
        public EntityState Delete(T entity)
        {
            var entityState = _dbSet.Remove(entity);
            return entityState.State;
        }

        public T Get<Tkey>(Tkey id)
        {
            return _dbSet.Find(id);
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet.AsQueryable();
        }

        public async Task<T> GetByIdAsync<Tkey>(Tkey id)
        {
            return await _dbSet.FindAsync(id);
        }

        public EntityState Update(T entity)
        {
            return _dbSet.Update(entity).State;
        }

    }
}
