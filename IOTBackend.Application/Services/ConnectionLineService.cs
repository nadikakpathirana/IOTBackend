using Microsoft.EntityFrameworkCore;

using IOTBackend.Application.Interfaces;
using IOTBackend.Domain.DbEntities;
using IOTBackend.Infrastructure.Interfaces;
using IOTBackend.Persistance;
using IOTBackend.Shared.Enums;
using IOTBackend.Shared.Responses;

namespace IOTBackend.Application.Services
{
    public class ConnectionLineService : IConnectionLineService
    {
        private readonly IUnitOfWork<AppDbContext> _unitOfWork;

        public ConnectionLineService(IUnitOfWork<AppDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<List<ConnectionLine>> GetConnectionLinesOfProjectAsync(Guid projectId)
        {
            var connectionLineRepository = _unitOfWork.GetRepository<ConnectionLine>();
            var connectionLines = await connectionLineRepository.FindByAsync(cl => cl.ProjectId == projectId);
            return connectionLines;
        }

        public async Task<ConnectionLine> GetConnectionLineAsync(Guid connectionLineId)
        {
            var connectionLineRepository = _unitOfWork.GetRepository<ConnectionLine>();
            var connectionLine = await connectionLineRepository.GetAsync(connectionLineId);
            return connectionLine;
        }

        public async Task<CommonActionResult<ConnectionLine>> CreateConnectionLineAsync(ConnectionLine connectionLine)
        {
            var response = new CommonActionResult<ConnectionLine>();
            var connectionLineRepository = _unitOfWork.GetRepository<ConnectionLine>();

            connectionLine.Id = new Guid();
            var result = await connectionLineRepository.AddAsync(connectionLine);
            _unitOfWork.Commit();

            response.Status = result == EntityState.Added ? ActionStatus.Success : ActionStatus.Failed;
            response.Entity = connectionLine;

            return response;
        }

        public async Task<CommonActionResult<ConnectionLine>> UpdateConnectionLineAsync(ConnectionLine connectionLine)
        {
            var response = new CommonActionResult<ConnectionLine>();
            var connectionLineRepository = _unitOfWork.GetRepository<ConnectionLine>();

            var existingConnectionLine = await connectionLineRepository.GetAsync(connectionLine.Id);
            if (existingConnectionLine == null)
            {
                response.Status = ActionStatus.NotFound;
                return response;
            }

            existingConnectionLine.FromDevice = connectionLine.FromDevice;
            existingConnectionLine.ToDevice = connectionLine.ToDevice;
            existingConnectionLine.Condition = connectionLine.Condition;
            existingConnectionLine.StartXCordinate = connectionLine.StartXCordinate;
            existingConnectionLine.StartYCordinate = connectionLine.StartYCordinate;
            existingConnectionLine.EndXCordinate = connectionLine.EndXCordinate;
            existingConnectionLine.EndYCordinate = connectionLine.EndYCordinate;

            var result = connectionLineRepository.Update(existingConnectionLine);
            _unitOfWork.Commit();

            response.Status = result == EntityState.Modified ? ActionStatus.Success : ActionStatus.Failed;
            response.Entity = existingConnectionLine;

            return response;
        }

        public async Task<CommonActionResult<ConnectionLine>> DeleteConnectionLineAsync(Guid connectionId)
        {
            var response = new CommonActionResult<ConnectionLine>();
            var connectionLineRepository = _unitOfWork.GetRepository<ConnectionLine>();

            var existingConnectionLine = await connectionLineRepository.GetAsync(connectionId);
            if (existingConnectionLine == null)
            {
                response.Status = ActionStatus.NotFound;
                return response;
            }

            var result = connectionLineRepository.Delete(existingConnectionLine);
            _unitOfWork.Commit();

            response.Status = result == EntityState.Deleted ? ActionStatus.Success : ActionStatus.Failed;
            response.Entity = existingConnectionLine;

            return response;
        }

        public bool IsExists(Guid id)
        {
            var connectionLineRepository = _unitOfWork.GetRepository<ConnectionLine>();
            return connectionLineRepository.Exists(connectionLine => connectionLine.Id == id);
        }
    }

}
