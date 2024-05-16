using IOTBackend.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IOTBackend.Infrastructure.Management
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;


        public Repository(DbContext context)
        {
            this._context = context;

            this._dbSet = context.Set<T>();
        }


        public virtual EntityState Add(T entity)
        {
            return this._dbSet.Add(entity).State;
        }


        public virtual T AddEntity(T entity)
        {
            var result = this._dbSet.Add(entity);
            return result.Entity;
        }


        public virtual async Task<EntityState> AddAsync(T entity)
        {
            var result = await this._dbSet.AddAsync(entity);
            return result.State;
        }


        public virtual async Task<T> AddEntityAsync(T entity)
        {
            var result = await this._dbSet.AddAsync(entity);
            return result.Entity;
        }


        public T Get<TKey>(TKey id)
        {
            return this._dbSet.Find(id);
        }


        public async Task<T> GetAsync<TKey>(TKey id)
        {
            return await this._dbSet.FindAsync(id);
        }


        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return this._dbSet.AsNoTracking().Where(predicate);
        }

        public async Task<List<T>> FindByAsync(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).ToList();
        }

        public IQueryable<T> GetAll()
        {
            return this._dbSet.AsNoTracking();
        }

        public bool Exists(Expression<Func<T, bool>> predicate)
        {
            return this._dbSet.Any(predicate);
        }

        public EntityState Delete(T entity)
        {
            return this._dbSet.Remove(entity).State;
        }

        public virtual EntityState Update(T entity)
        {
            return this._dbSet.Update(entity).State;
        }
    }
}
