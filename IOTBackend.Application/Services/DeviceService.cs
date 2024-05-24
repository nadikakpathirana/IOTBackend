using Microsoft.EntityFrameworkCore;

using IOTBackend.Application.Interfaces;
using IOTBackend.Domain.DbEntities;
using IOTBackend.Infrastructure.Interfaces;
using IOTBackend.Persistance;
using IOTBackend.Shared.Enums;
using IOTBackend.Shared.Responses;

namespace IOTBackend.Application.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly IUnitOfWork<AppDbContext> _unitOfWork;

        public DeviceService(IUnitOfWork<AppDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }
        
        public async Task<List<Device>> GetAll()
        {
            var deviceRepository = _unitOfWork.GetRepository<Device>();
            var devices = await deviceRepository.GetAll().ToListAsync();
            return devices;
        }

        public async Task<List<Device>> GetDevicesOfUserAsync(Guid userId)
        {
            var deviceRepository = _unitOfWork.GetRepository<Device>();
            var devices = await deviceRepository.FindByAsync(d => d.UserId == userId);
            return devices;
        }

        public async Task<CommonActionResult<Device>> GetDeviceAsync(Guid deviceId)
        {
            var response = new CommonActionResult<Device>();
            var deviceRepository = _unitOfWork.GetRepository<Device>();

            var device = await deviceRepository.GetAsync(deviceId);
            if (device == null)
            {
                response.Status = ActionStatus.NotFound;
                return response;
            }

            response.Status = ActionStatus.Success;
            response.Entity = device;
            return response;
        }

        public async Task<CommonActionResult<Device>> AddDeviceAsync(Device device)
        {
            var response = new CommonActionResult<Device>();
            var deviceRepository = _unitOfWork.GetRepository<Device>();

            device.Id = new Guid();
            device.Created = DateTime.UtcNow;
            var result = await deviceRepository.AddAsync(device);
            _unitOfWork.Commit();

            response.Status = result.Item1 == EntityState.Added ? ActionStatus.Success : ActionStatus.Failed;
            response.Entity = result.Item2;
            return response;
        }

        public async Task<CommonActionResult<Device>> UpdateDeviceAsync(DeviceUpdateDto device)
        {
            var response = new CommonActionResult<Device>();
            var deviceRepository = _unitOfWork.GetRepository<Device>();

            var existingDevice = await deviceRepository.GetAsync(device.Id);
            if (existingDevice == null)
            {
                response.Status = ActionStatus.NotFound;
                return response;
            }

            existingDevice.Name = device.Name;
            existingDevice.DeviceType = device.DeviceType;

            var result = deviceRepository.Update(existingDevice);
            _unitOfWork.Commit();

            response.Status = result.Item1 == EntityState.Modified ? ActionStatus.Success : ActionStatus.Failed;
            response.Entity = result.Item2;
            return response;
        }

        public async Task<CommonActionResult<Device>> DeleteDeviceAsync(Guid deviceId)
        {
            var response = new CommonActionResult<Device>();
            var deviceRepository = _unitOfWork.GetRepository<Device>();

            var existingDevice = await deviceRepository.GetAsync(deviceId);
            if (existingDevice == null)
            {
                response.Status = ActionStatus.NotFound;
                return response;
            }

            var result = deviceRepository.Delete(existingDevice);
            _unitOfWork.Commit();

            response.Status = result == EntityState.Deleted ? ActionStatus.Success : ActionStatus.Failed;
            return response;
        }

        public bool IsExists(Guid id)
        {
            var deviceRepository = _unitOfWork.GetRepository<Device>();
            return deviceRepository.Exists(device => device.Id == id);
        }
    }

}
