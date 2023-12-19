using System.Linq.Expressions;
using IOTBackend.API.DataLayer.Entities.Base;

namespace IOTBackend.API.DataLayer.Repository.Interfaces
{
    public interface IRepository<T> where T : EntityBase
    {
        Task<T?> GetByIdAsync(int id, string? navigationsToInclude = null);
        Task<IReadOnlyList<T>> GetAllAsync(params string[] navigationsToInclude);
        Task<IReadOnlyList<T>> GetByCondition(Expression<Func<T, bool>> condition);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(int id, T updatedEntity);
        Task<T> UpdateAsync(int id, object updatedEntity);
        Task<bool> DeleteAsync(int id);
        IQueryable<T> GetQueryable();
    }
}
