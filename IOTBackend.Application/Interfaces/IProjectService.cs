using IOTBackend.Domain.DbEntities;
using IOTBackend.Shared.Responses;

namespace IOTBackend.Application.Interfaces
{
    public interface IProjectService
    {
        Task<List<Project>> GetAllProjectsOfUserAsync(Guid userId);
        Task<List<Project>> GetProjectAsync(Guid projectId);
        Task<CommonActionResult<Project>> CreateProjectAsync(Project project);
        Task<CommonActionResult<Project>> UpdateProjectAsync(Project project);
        Task<CommonActionResult<Project>> DeleteProjectAsync(Guid projectId);
        bool IsExists(Guid id);
    }
}
