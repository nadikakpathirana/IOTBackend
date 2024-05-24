using IOTBackend.Domain.DbEntities;
using IOTBackend.Domain.Dtos;
using IOTBackend.Shared.Responses;
namespace IOTBackend.Application.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetUserAsync(string username);
        Task<CommonActionResult<User>> AddUserAsync(User user);
        Task<CommonActionResult<User>> UpdateUserAsync(Guid userId, UserUpdateDto user);
        Task<CommonActionResult<User>> DeleteUserAsync(Guid userId);
        Dictionary<string, string> GetUserClaims();
        bool IsEmailExists(string email);
        bool IsUsernameExists(string username);

    }
}
