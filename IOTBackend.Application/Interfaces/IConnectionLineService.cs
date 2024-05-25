using IOTBackend.Domain.DbEntities;
using IOTBackend.Domain.Dtos;
using IOTBackend.Shared.Responses;

namespace IOTBackend.Application.Interfaces
{
    public interface IConnectionLineService
    {
        Task<List<ConnectionLine>> GetAll();
        Task<List<ConnectionLine>> GetConnectionLinesOfProjectAsync(Guid projectId);
        Task<List<ConnectionLine>> GetConnectionLinesOfDevicesBeginWith(Guid deviceId);
        Task<ConnectionLine?> GetConnectionLineAsync(Guid projectId);
        Task<CommonActionResult<ConnectionLine>> CreateConnectionLineAsync(ConnectionLineCreateDto connectionLine);
        Task<CommonActionResult<ConnectionLine>> UpdateConnectionLineAsync(Guid connectionId, ConnectionLineUpdateDto connectionLine);
        Task<CommonActionResult<ConnectionLine>> DeleteConnectionLineAsync(Guid connectionId);
        bool IsExists(Guid id);
    }
}
