using IOTBackend.Domain.DbEntities;
using IOTBackend.Domain.Dtos;
using IOTBackend.Shared.Responses;

namespace IOTBackend.Application.Interfaces
{
    public interface IAPIKeyService
    {
        Task<List<ApiKey>> GetAll();
        Task<ApiKey?> GetKeyOfAUser(Guid userId);
        Task<ApiKey?> GetKey(Guid keyId);
        Task<CommonActionResult<ApiKey>> CreateKey(ApiKeyAddDto apiKeyAddDto);
        Task<CommonActionResult<ApiKey>> UpdateKey(Guid keyId, ApiKeyUpdateDto apiKeyUpdateDto);
        Task<CommonActionResult<ApiKey>> DeleteKey(Guid keyId);
        bool IsExists(Guid id);

    }
}
