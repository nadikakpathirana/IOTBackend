using IOTBackend.Domain.DbEntities;
using IOTBackend.Shared.Responses;

namespace IOTBackend.Application.Interfaces
{
    public interface IAPIKeyService
    {
        Task<APIKey?> GetKeyOfAUser(Guid keyId);
        Task<APIKey> GetKey(Guid keyId);
        Task<CommonActionResult<APIKey>> CreateKey(APIKey key);
        Task<CommonActionResult<APIKey>> UpdateKey(APIKey key);
        Task<CommonActionResult<APIKey>> DeleteKey(Guid keyId);
        bool IsExists(Guid id);

    }
}
