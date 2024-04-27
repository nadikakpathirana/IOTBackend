using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOTBackend.Infrastructure.Interfaces
{
    public interface IUnitOfWork<TContext> where TContext : DbContext, IDisposable, new()
    {

        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;

        int Commit();

        Task CommitAsync();

        bool DetachAllEntities();
    }
}
