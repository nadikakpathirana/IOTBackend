using AutoMapper;
using Microsoft.EntityFrameworkCore;

using IOTBackend.Application.Interfaces;
using IOTBackend.Domain.DbEntities;
using IOTBackend.Domain.Dtos;
using IOTBackend.Infrastructure.Interfaces;
using IOTBackend.Persistance;
using IOTBackend.Shared.Enums;
using IOTBackend.Shared.Responses;

namespace IOTBackend.Application.Services
{
    public class ApiKeyService : IAPIKeyService
    {
        private readonly IUnitOfWork<AppDbContext> _unitOfWork;
        private readonly IMapper _mapper;

        public ApiKeyService(IUnitOfWork<AppDbContext> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper;
        }
        
        public async Task<List<ApiKey>> GetAll()
        {
            var apiKeyRepository = _unitOfWork.GetRepository<ApiKey>();
            var apiKeys = await apiKeyRepository.GetAll().ToListAsync();

            return apiKeys;
        }

        public async Task<ApiKey?> GetKeyOfAUser(Guid userId)
        {
            var apiKeyRepository = _unitOfWork.GetRepository<ApiKey>();
            var apiKey = await apiKeyRepository.FindByAsync(x => x.UserId == userId);
            
            if (apiKey.Count > 0)
            {
                apiKey[0] = await IncludeOwner(apiKey[0]);
                return apiKey[0];
            }
            return null;
        }
        
        public async Task<ApiKey?> GetKey(Guid keyId)
        {
            var apiKeyRepository = _unitOfWork.GetRepository<ApiKey>();
            var apiKey = await apiKeyRepository.FindByAsync(x => x.Id == keyId);
            
            if (apiKey.Count > 0)
            {
                apiKey[0] = await IncludeOwner(apiKey[0]);
                return apiKey[0];
            }

            return null;
        }

        public async Task<CommonActionResult<ApiKey>> CreateKey(ApiKeyAddDto apiKeyAddDto)
        {
            var response = new CommonActionResult<ApiKey>();
            var apiKeyRepository = _unitOfWork.GetRepository<ApiKey>();

            var newKey = _mapper.Map<ApiKey>(apiKeyAddDto);
            
            newKey.Id = new Guid();
            newKey.Created = DateTime.UtcNow;
            
            var result = await apiKeyRepository.AddAsync(newKey);
            _unitOfWork.Commit();

            response.Status = result.Item1 == EntityState.Added ? ActionStatus.Success : ActionStatus.Failed;
            response.Entity = result.Item2;

            return response;
        }

        public async Task<CommonActionResult<ApiKey>> UpdateKey(Guid keyId, ApiKeyUpdateDto apiKeyUpdateDto)
        {
            var response = new CommonActionResult<ApiKey>();
            
            var apiKeyRepository = _unitOfWork.GetRepository<ApiKey>();
            var existingKey = await apiKeyRepository.GetAsync(keyId);
            
            if (existingKey == null)
            {
                response.Status = ActionStatus.NotFound;
                return response;
            }

            existingKey.Name = apiKeyUpdateDto.Name;

            var result = apiKeyRepository.Update(existingKey);
            _unitOfWork.Commit();

            response.Status = result.Item1 == EntityState.Modified ? ActionStatus.Success : ActionStatus.Failed;
            response.Entity = result.Item2;

            return response;
        }

        public async Task<CommonActionResult<ApiKey>> DeleteKey(Guid keyId)
        {
            var response = new CommonActionResult<ApiKey>();
            var apiKeyRepository = _unitOfWork.GetRepository<ApiKey>();

            var existingKey = await apiKeyRepository.GetAsync(keyId);
            if (existingKey == null)
            {
                response.Status = ActionStatus.NotFound;
                return response;
            }

            var result = apiKeyRepository.Delete(existingKey);
            _unitOfWork.Commit();

            response.Status = result == EntityState.Deleted ? ActionStatus.Success : ActionStatus.Failed;
            response.Entity = existingKey;

            return response;
        }

        public bool IsExists(Guid id)
        {
            var apiKeyRepository = _unitOfWork.GetRepository<ApiKey>();
            return apiKeyRepository.Exists(apiKey => apiKey.Id == id);
        }

        private async Task<ApiKey> IncludeOwner(ApiKey apiKey)
        {
            // Fetch the user data for the owner
            var userRepository = _unitOfWork.GetRepository<User>();
            var owner = await userRepository.GetAsync(apiKey.UserId);
            apiKey.Owner = owner;
            return apiKey;
        }
    }
}
