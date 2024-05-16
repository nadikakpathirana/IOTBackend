using Microsoft.EntityFrameworkCore;

using IOTBackend.Application.Interfaces;
using IOTBackend.Domain.DbEntities;
using IOTBackend.Infrastructure.Interfaces;
using IOTBackend.Persistance;
using IOTBackend.Shared.Enums;
using IOTBackend.Shared.Responses;

namespace IOTBackend.Application.Services
{
    public class APIKeyService : IAPIKeyService
    {
        private readonly IUnitOfWork<AppDbContext> _unitOfWork;

        public APIKeyService(IUnitOfWork<AppDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<APIKey> GetKeyOfAUser(Guid userId)
        {
            var apiKeyRepository = _unitOfWork.GetRepository<APIKey>();
            var apiKey = await apiKeyRepository.FindByAsync(x => x.UserId == userId);
            if (apiKey.Count > 0)
            {
                apiKey[0] = await IncludeOwner(apiKey[0]);
            }
            return apiKey[0];
        }

        public async Task<CommonActionResult<APIKey>> CreateKey(APIKey key)
        {
            var response = new CommonActionResult<APIKey>();
            var apiKeyRepository = _unitOfWork.GetRepository<APIKey>();

            key.Id = new Guid();
            var result = await apiKeyRepository.AddAsync(key);
            _unitOfWork.Commit();

            response.Status = result == EntityState.Added ? ActionStatus.Success : ActionStatus.Failed;
            response.Entity = key;

            return response;
        }

        public async Task<CommonActionResult<APIKey>> UpdateKey(APIKey key)
        {
            var response = new CommonActionResult<APIKey>();
            var apiKeyRepository = _unitOfWork.GetRepository<APIKey>();

            var existingKey = await apiKeyRepository.GetAsync(key.Id);
            if (existingKey == null)
            {
                response.Status = ActionStatus.NotFound;
                return response;
            }

            existingKey.Name = key.Name;

            var result = apiKeyRepository.Update(existingKey);
            _unitOfWork.Commit();

            response.Status = result == EntityState.Modified ? ActionStatus.Success : ActionStatus.Failed;
            response.Entity = existingKey;

            return response;
        }

        public async Task<CommonActionResult<APIKey>> DeleteKey(Guid keyId)
        {
            var response = new CommonActionResult<APIKey>();
            var apiKeyRepository = _unitOfWork.GetRepository<APIKey>();

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
            var apiKeyRepository = _unitOfWork.GetRepository<APIKey>();
            return apiKeyRepository.Exists(apiKey => apiKey.Id == id);
        }

        private async Task<APIKey> IncludeOwner(APIKey apiKey)
        {
            // Fetch the user data for the owner
            var userRepository = _unitOfWork.GetRepository<User>();
            var owner = await userRepository.GetAsync(apiKey.UserId);
            apiKey.Owner = owner;
            return apiKey;
        }
    }
}
