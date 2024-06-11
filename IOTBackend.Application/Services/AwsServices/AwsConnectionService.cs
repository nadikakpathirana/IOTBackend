using Microsoft.EntityFrameworkCore;

using IOTBackend.Application.Services.Interfaces;
using IOTBackend.Domain.DbEntities.Aws;
using IOTBackend.Infrastructure.Interfaces;
using IOTBackend.Persistance;

namespace IOTBackend.Application.Services
{
    public class AwsConnectionService : IAwsConnectionService
    {
        private readonly IUnitOfWork<AwsDbContext> _unitOfWork;

        public AwsConnectionService(IUnitOfWork<AwsDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }
        
        public async Task<List<Connection>> GetAll()
        {
            var connectionRepository = _unitOfWork.GetRepository<Connection>();
            var connections = await connectionRepository.GetAll().ToListAsync();
            return connections;
        }
    }

}
