using IOTBackend.Domain.DbEntities;
using IOTBackend.Domain.Dtos;
using IOTBackend.Shared.Responses;

namespace IOTBackend.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ApiRequestResult<User>> Register(UserRegisterDto userRegisterDto);
        Task<User> Login(UserLoginDto userLoginDto);
        string GenerateToken(User user);
    }
}
