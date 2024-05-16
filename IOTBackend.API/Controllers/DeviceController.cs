using IOTBackend.Application.Interfaces;
using IOTBackend.Domain.DbEntities;
using IOTBackend.Shared.Enums;
using IOTBackend.Shared.Responses;
using Microsoft.AspNetCore.Mvc;

namespace IOTBackend.API.Controllers
{
    [ApiController]
    [Route("api/devices")]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceService _deviceService;

        public DeviceController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<Device>>> GetDevicesOfUser(Guid userId)
        {
            var devices = await _deviceService.GetDevicesOfUserAsync(userId);

            var response = new ApiRequestResult<List<Device>>
            {
                Status = true,
                Message = "Devices fetched successfully",
                Data = devices
            };

            return Ok(response);
        }

        [HttpGet("{deviceId}")]
        public async Task<ActionResult<ApiRequestResult<Device>>> GetDevice(Guid deviceId)
        {
            var result = await _deviceService.GetDeviceAsync(deviceId);

            if (result.Status == ActionStatus.NotFound)
            {
                var errorResponse = new ApiRequestResult<Device>
                {
                    Status = false,
                    Message = "Device not found"
                };
                return NotFound(errorResponse);
            }

            var response = new ApiRequestResult<Device>
            {
                Status = true,
                Message = "Device fetched successfully",
                Data = result.Entity
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ApiRequestResult<Device>>> AddDevice(Device device)
        {
            var result = await _deviceService.AddDeviceAsync(device);

            if (result.Status == ActionStatus.Failed)
            {
                var errorResponse = new ApiRequestResult<Device>
                {
                    Status = false,
                    Message = "Failed to add device"
                };
                return BadRequest(errorResponse);
            }

            var response = new ApiRequestResult<Device>
            {
                Status = true,
                Message = "Device added successfully",
                Data = device
            };

            return Created($"GetDevice/{result.Entity.Id}", response);
        }

        [HttpPut("{deviceId}")]
        public async Task<ActionResult<ApiRequestResult<Device>>> UpdateDevice(Guid deviceId, Device device)
        {
            device.Id = deviceId;
            var result = await _deviceService.UpdateDeviceAsync(device);

            if (result.Status == ActionStatus.NotFound)
            {
                var errorResponse = new ApiRequestResult<Device>
                {
                    Status = false,
                    Message = "Device not found"
                };
                return NotFound(errorResponse);
            }

            if (result.Status == ActionStatus.Failed)
            {
                var errorResponse = new ApiRequestResult<Device>
                {
                    Status = false,
                    Message = "Failed to update device"
                };
                return BadRequest(errorResponse);
            }

            var response = new ApiRequestResult<Device>
            {
                Status = true,
                Message = "Device updated successfully",
                Data = device
            };

            return Ok(response);
        }

        [HttpDelete("{deviceId}")]
        public async Task<ActionResult<ApiRequestResult<Device>>> DeleteDevice(Guid deviceId)
        {
            var result = await _deviceService.DeleteDeviceAsync(deviceId);

            if (result.Status == ActionStatus.NotFound)
            {
                var errorResponse = new ApiRequestResult<Device>
                {
                    Status = false,
                    Message = "Device not found"
                };
                return NotFound(errorResponse);
            }

            if (result.Status == ActionStatus.Failed)
            {
                var errorResponse = new ApiRequestResult<Device>
                {
                    Status = false,
                    Message = "Failed to delete device"
                };
                return BadRequest(errorResponse);
            }

            var response = new ApiRequestResult<Device>
            {
                Status = true,
                Message = "Device deleted successfully",
                Data = result.Entity
            };

            return Ok(response);
        }
    }
}
