using AutoMapper;
using IOTBackend.Application.Interfaces;
using IOTBackend.Domain.DbEntities;
using IOTBackend.Domain.Dtos;
using IOTBackend.Infrastructure.Interfaces;
using IOTBackend.Persistance;
using IOTBackend.Shared.Enums;
using IOTBackend.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace IOTBackend.Application.Services
{
    public class DeviceInstanceService : IDeviceInstanceService
    {
        private readonly IUnitOfWork<AppDbContext> _unitOfWork;
        public readonly IMapper _mapper;

        public DeviceInstanceService(IUnitOfWork<AppDbContext> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper;
        }
        
        public async Task<List<DeviceInstance>> GetAll()
        {
            var deviceInstanceRepository = _unitOfWork.GetRepository<DeviceInstance>();
            var deviceInstances = await deviceInstanceRepository.GetAll().ToListAsync();
            return deviceInstances;
        }
        
        public async Task<List<DeviceInstance>> GetDeviceInstancesesAsync(Guid projectId)
        {
            var deviceInstanceRepository = _unitOfWork.GetRepository<DeviceInstance>();
            var deviceInstances = await deviceInstanceRepository.FindByAsync(di => di.ProjectId == projectId);
            return deviceInstances;
        }

        public async Task<DeviceInstance?> GetDeviceInstanceAsync(Guid deviceInstanceId)
        {
            var deviceInstanceRepository = _unitOfWork.GetRepository<DeviceInstance>();
            return await deviceInstanceRepository.GetAsync(deviceInstanceId);
        }

        public async Task<CommonActionResult<DeviceInstance>> AddDeviceInstanceAsync(DeviceInstanceCreateDto deviceInstance)
        {
            var response = new CommonActionResult<DeviceInstance>();
            var deviceInstanceRepository = _unitOfWork.GetRepository<DeviceInstance>();

            var newDeviceInstance = _mapper.Map<DeviceInstance>(deviceInstance);

            newDeviceInstance.Id = new Guid();
            newDeviceInstance.Created = DateTime.UtcNow;
            
            var result = await deviceInstanceRepository.AddAsync(newDeviceInstance);
            _unitOfWork.Commit();

            response.Status = result.Item1 == EntityState.Added ? ActionStatus.Success : ActionStatus.Failed;
            response.Entity = result.Item2;
            return response;
        }

        public async Task<CommonActionResult<DeviceInstance>> UpdateDeviceInstanceAsync(Guid deviceInstanceId, DeviceInstanceUpdateDto deviceInstance)
        {
            var response = new CommonActionResult<DeviceInstance>();
            var deviceInstanceRepository = _unitOfWork.GetRepository<DeviceInstance>();

            var existingDeviceInstance = await deviceInstanceRepository.GetAsync(deviceInstanceId);
            if (existingDeviceInstance == null)
            {
                response.Status = ActionStatus.NotFound;
                return response;
            }

            // Update relevant properties
            existingDeviceInstance.XCordinate = deviceInstance.XCordinate;
            existingDeviceInstance.YCordinate = deviceInstance.YCordinate;

            var result = deviceInstanceRepository.Update(existingDeviceInstance);
            _unitOfWork.Commit();

            response.Status = result.Item1 == EntityState.Modified ? ActionStatus.Success : ActionStatus.Failed;
            response.Entity = result.Item2;
            return response;
        }

        public async Task<CommonActionResult<DeviceInstance>> DeleteDeviceInstanceAsync(Guid deviceInstanceId)
        {
            var response = new CommonActionResult<DeviceInstance>();
            var deviceInstanceRepository = _unitOfWork.GetRepository<DeviceInstance>();

            var existingDeviceInstance = await deviceInstanceRepository.GetAsync(deviceInstanceId);
            if (existingDeviceInstance == null)
            {
                response.Status = ActionStatus.NotFound;
                return response;
            }

            var result = deviceInstanceRepository.Delete(existingDeviceInstance);
            _unitOfWork.Commit();

            response.Status = result == EntityState.Deleted ? ActionStatus.Success : ActionStatus.Failed;
            response.Entity = existingDeviceInstance;
            return response;
        }

        public bool IsExists(Guid id)
        {
            var deviceInstanceRepository = _unitOfWork.GetRepository<DeviceInstance>();
            return deviceInstanceRepository.Exists(deviceInstance => deviceInstance.Id == id);
        }
    }

}
