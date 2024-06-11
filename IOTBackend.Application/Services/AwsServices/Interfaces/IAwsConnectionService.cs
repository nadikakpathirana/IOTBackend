using IOTBackend.Domain.DbEntities.Aws;

namespace IOTBackend.Application.Services.Interfaces {
    public interface IAwsConnectionService
    {
        Task<List<Connection>> GetAll();
    }
}