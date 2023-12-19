using IOTBackend.API.DataLayer.Context;
using IOTBackend.API.DataLayer.Repository.Interfaces;

namespace IOTBackend.API.DataLayer.Repository.Impl
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly DefaultDBContext dbContext;
        private readonly IUserRepository _user;

        public RepositoryWrapper(DefaultDBContext dbContext)
        {
            this.dbContext = dbContext;
            _user = new UserRepository(dbContext);
        }


        public IUserRepository UserRepository => _user;

        public async Task<int> SaveAsync()
        {
            return await dbContext.SaveChangesAsync();
        }
    }
}
