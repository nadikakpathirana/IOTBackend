using IOTBackend.Domain.DbEntities;
using IOTBackend.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOTBackend.Application.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUserAsync(Guid userId);
        Task<CommonActionResult> AddUserAsync(User user);
        Task<CommonActionResult> UpdateUserAsync(Guid userId, User user);
        Task<CommonActionResult> DeleteUserAsync(Guid userId);
    }
}
