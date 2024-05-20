using IOTBackend.Application.Interfaces;
using IOTBackend.Domain.DbEntities;
using IOTBackend.Shared.Enums;
using IOTBackend.Shared.Responses;
using Microsoft.AspNetCore.Mvc;

namespace IOTBackend.API.Controllers
{
    [ApiController]
    [Route("api/deviceinstances")]
    public class DeviceInstanceController : ControllerBase
    {
        private readonly IDeviceInstanceService _deviceInstanceService;

        public DeviceInstanceController(IDeviceInstanceService deviceInstanceService)
        {
            _deviceInstanceService = deviceInstanceService;
        }

        [HttpGet("{projectId}")]
        public async Task<ActionResult<List<DeviceInstance>>> GetDeviceInstances(Guid projectId)
        {
            var deviceInstances = await _deviceInstanceService.GetDeviceInstancesesAsync(projectId);

            var response = new ApiRequestResult<List<DeviceInstance>>
            {
                Status = true,
                Message = "Device instances fetched successfully",
                Data = deviceInstances
            };

            return Ok(response);
        }

        [HttpGet("{deviceInstanceId}")]
        public async Task<ActionResult<ApiRequestResult<DeviceInstance>>> GetDeviceInstance(Guid deviceInstanceId)
        {

            var deviceInstance = await _deviceInstanceService.GetDeviceInstanceAsync(deviceInstanceId);

            if (deviceInstance == null)
            {
                var errorResponse = new ApiRequestResult<DeviceInstance>
                {
                    Message = "Device instance not found"
                };
                return NotFound(errorResponse);
            }

            var response = new ApiRequestResult<DeviceInstance>
            {
                Status = true,
                Message = "Device instance fetched successfully",
                Data = deviceInstance
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ApiRequestResult<DeviceInstance>>> AddDeviceInstance(DeviceInstance deviceInstance)
        {

            var result = await _deviceInstanceService.AddDeviceInstanceAsync(deviceInstance);

            if (result.Status == ActionStatus.Error)
            {
                var errorResponse = new ApiRequestResult<DeviceInstance>
                {
                    Status = false,
                    Message = "Failed to add device instance"
                };
                return BadRequest(errorResponse);
            }

            var response = new ApiRequestResult<DeviceInstance>
            {
                Status = true,
                Message = "Device instance added successfully",
                Data = deviceInstance
            };

            return Created($"GetDeviceInstance/{result.Entity.Id}", response);
        }

        [HttpPut("{deviceInstanceId}")]
        public async Task<ActionResult<ApiRequestResult<DeviceInstance>>> UpdateDeviceInstance(Guid deviceInstanceId, DeviceInstance deviceInstance)
        {
            var result = await _deviceInstanceService.UpdateDeviceInstanceAsync(deviceInstance);

            if (result.Status == ActionStatus.NotFound)
            {
                var errorResponse = new ApiRequestResult<DeviceInstance>
                {
                    Status = false,
                    Message = "Device instance not found"
                };
                return NotFound(errorResponse);
            }

            if (result.Status == ActionStatus.Failed)
            {
                var errorResponse = new ApiRequestResult<DeviceInstance>
                {
                    Status = false,
                    Message = "Failed to update device instance"
                };
                return BadRequest(errorResponse);
            }

            var response = new ApiRequestResult<DeviceInstance>
            {
                Status = true,
                Message = "Device instance updated successfully",
                Data = deviceInstance
            };

            return Ok(response);
        }

        [HttpDelete("{deviceInstanceId}")]
        public async Task<ActionResult<ApiRequestResult<DeviceInstance>>> DeleteDeviceInstance(Guid deviceInstanceId)
        {
            var result = await _deviceInstanceService.DeleteDeviceInstanceAsync(deviceInstanceId);

            if (result.Status == ActionStatus.NotFound)
            {
                var errorResponse = new ApiRequestResult<DeviceInstance>
                {
                    Status = false,
                    Message = "Device instance not found"
                };
                return NotFound(errorResponse);
            }

            if (result.Status == ActionStatus.Failed)
            {
                var errorResponse = new ApiRequestResult<DeviceInstance>
                {
                    Status = false,
                    Message = "Failed to delete device instance"
                };
                return BadRequest(errorResponse);
            }

            var response = new ApiRequestResult<DeviceInstance>
            {
                Status = true,
                Message = "Device instance deleted successfully",
                Data = result.Entity
            };


            return Ok(response);
        }
    }
}
