using IOTBackend.Domain.DbEntities;
using IOTBackend.Shared.Responses;

namespace IOTBackend.Application.Interfaces
{
    public interface IConnectionLineService
    {
        Task<List<ConnectionLine>> GetConnectionLinesOfProjectAsync(Guid projectId);
        Task<ConnectionLine> GetConnectionLineAsync(Guid projectId);
        Task<CommonActionResult<ConnectionLine>> CreateConnectionLineAsync(ConnectionLine connectionLine);
        Task<CommonActionResult<ConnectionLine>> UpdateConnectionLineAsync(ConnectionLine connectionLine);
        Task<CommonActionResult<ConnectionLine>> DeleteConnectionLineAsync(Guid connectionId);
        bool IsExists(Guid id);
    }
}
