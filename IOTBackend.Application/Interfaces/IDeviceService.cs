using IOTBackend.Domain.DbEntities;
using IOTBackend.Shared.Responses;

namespace IOTBackend.Application.Interfaces
{
    public interface IDeviceService
    {
        Task<List<Device>> GetDevicesOfUserAsync(Guid UserId);
        Task<CommonActionResult<Device>> GetDeviceAsync(Guid DeviceId);
        Task<CommonActionResult<Device>> AddDeviceAsync(Device Device);
        Task<CommonActionResult<Device>> UpdateDeviceAsync(Device Device);
        Task<CommonActionResult<Device>> DeleteDeviceAsync(Guid DeviceId);
        bool IsExists(Guid id);
    }
}
