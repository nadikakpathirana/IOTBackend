using IOTBackend.Domain.DbEntities;
using IOTBackend.Shared.Responses;

namespace IOTBackend.Application.Interfaces
{
    public interface IDeviceInstanceService
    {
        Task<List<DeviceInstance>> GetDeviceInstancesesAsync(Guid projectId);
        Task<DeviceInstance> GetDeviceInstanceAsync(Guid deviceInstanceId);
        Task<CommonActionResult<DeviceInstance>> AddDeviceInstanceAsync(DeviceInstance deviceInstance);
        Task<CommonActionResult<DeviceInstance>> UpdateDeviceInstanceAsync(DeviceInstance deviceInstance);
        Task<CommonActionResult<DeviceInstance>> DeleteDeviceInstanceAsync(Guid deviceInstanceId);
        bool IsExists(Guid id);
    }
}
