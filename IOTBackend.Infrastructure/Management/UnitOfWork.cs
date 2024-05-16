using IOTBackend.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IOTBackend.Infrastructure.Management
{
    public sealed class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext, IDisposable, new()
    {
        private readonly TContext _dbContext;

        private Dictionary<Type, object>? repositories;

        public UnitOfWork()
        {
            _dbContext = DbContextFactory.CreateDbContext<TContext>();
            try
            {
                _dbContext.Database.OpenConnection();
                _dbContext.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in UnitOfWork");
                Console.WriteLine(ex);
            }
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            if (repositories == null)
            {
                repositories = new Dictionary<Type, object>();
            }

            var type = typeof(TEntity);
            if (!repositories.ContainsKey(type))
            {
                repositories[type] = new Repository<TEntity>(_dbContext);
            }

            return (IRepository<TEntity>)repositories[type];
        }

        public int Commit()
        {
            return _dbContext.SaveChanges();
        }


        public async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
            return;
        }

        public bool DetachAllEntities()
        {
            var changedEntriesCopy = _dbContext.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted)
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;

            return true;
        }

        public void Dispose()
        {
            Dispose(true);

            // ReSharper disable once GCSuppressFinalizeForTypeWithoutDestructor
            GC.SuppressFinalize(obj: this);
        }


        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_dbContext != null)
                {
                    _dbContext.Dispose();
                }
            }
        }
    }
}
