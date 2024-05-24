using IOTBackend.Domain.DbEntities;
using IOTBackend.Domain.Dtos;
using IOTBackend.Shared.Responses;

namespace IOTBackend.Application.Interfaces
{
    public interface IProjectService
    {
        Task<List<Project>> GetAll();
        Task<List<Project>> GetAllProjectsOfUserAsync(Guid userId);
        Task<List<Project>> GetProjectAsync(Guid projectId);
        Task<CommonActionResult<Project>> CreateProjectAsync(ProjectCreateDto project);
        Task<CommonActionResult<Project>> UpdateProjectAsync(Guid projectId, ProjectUpdateDto project);
        Task<CommonActionResult<Project>> DeleteProjectAsync(Guid projectId);
        bool IsExists(Guid id);
    }
}
