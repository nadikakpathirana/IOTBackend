using IOTBackend.Domain.DbEntities;
using IOTBackend.Domain.Dtos;
using IOTBackend.Shared.Responses;

namespace IOTBackend.Application.Interfaces
{
    public interface IDeviceInstanceService
    {
        Task<List<DeviceInstance>> GetAll();
        Task<List<DeviceInstance>> GetDeviceInstancesesAsync(Guid projectId);
        Task<DeviceInstance?> GetDeviceInstanceAsync(Guid deviceInstanceId);
        Task<CommonActionResult<DeviceInstance>> AddDeviceInstanceAsync(DeviceInstanceCreateDto deviceInstance);
        Task<CommonActionResult<DeviceInstance>> UpdateDeviceInstanceAsync(Guid deviceInstanceID, DeviceInstanceUpdateDto deviceInstance);
        Task<CommonActionResult<DeviceInstance>> DeleteDeviceInstanceAsync(Guid deviceInstanceId);
        bool IsExists(Guid id);
    }
}
