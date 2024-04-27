using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IOTBackend.Infrastructure.Interfaces
{
    public interface IRepository<T>
    {

        EntityState Add(T entity);


        T AddEntity(T entity);


        Task<EntityState> AddAsync(T entity);

        Task<T> AddEntityAsync(T entity);


        T Get<TKey>(TKey id);


        Task<T> GetAsync<TKey>(TKey id);


        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);


        IQueryable<T> GetAll();

        bool Exists(Expression<Func<T, bool>> predicate);

        EntityState Delete(T entity);

        EntityState Update(T entity);
    }
}
