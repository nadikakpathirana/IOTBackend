namespace IOTBackend.API.DataLayer.Repository.Interfaces
{
    public interface IRepositoryWrapper
    {
        public IUserRepository UserRepository { get; }
        Task<int> SaveAsync();
    }
}