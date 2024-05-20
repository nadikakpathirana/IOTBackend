using IOTBackend.Application.Interfaces;
using IOTBackend.Domain.DbEntities;
using IOTBackend.Infrastructure.Interfaces;
using IOTBackend.Persistance;
using IOTBackend.Shared.Enums;
using IOTBackend.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace IOTBackend.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork<AppDbContext> _unitOfWork;

        public ProjectService(IUnitOfWork<AppDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<List<Project>> GetAllProjectsOfUserAsync(Guid userId)
        {
            var projectRepository = _unitOfWork.GetRepository<Project>();
            var projects = await projectRepository.FindByAsync(p => p.UserId == userId);
            return projects;
        }

        public async Task<List<Project>> GetProjectAsync(Guid projectId)
        {
            var projectRepository = _unitOfWork.GetRepository<Project>();
            var project = await projectRepository.FindByAsync(p => p.Id == projectId);
            return project;
        }

        public async Task<CommonActionResult<Project>> CreateProjectAsync(Project project)
        {
            var response = new CommonActionResult<Project>();
            var projectRepository = _unitOfWork.GetRepository<Project>();

            project.Id = new Guid();
            var result = await projectRepository.AddAsync(project);
            _unitOfWork.Commit();

            response.Status = result.Item1 == EntityState.Added ? ActionStatus.Success : ActionStatus.Failed;
            response.Entity = result.Item2;
            return response;
        }

        public async Task<CommonActionResult<Project>> UpdateProjectAsync(Project project)
        {
            var response = new CommonActionResult<Project>();
            var projectRepository = _unitOfWork.GetRepository<Project>();

            var existingProject = await projectRepository.GetAsync(project.Id);
            if (existingProject == null)
            {
                response.Status = ActionStatus.NotFound;
                return response;
            }

            // Update relevant properties
            existingProject.Name = project.Name;

            var result = projectRepository.Update(existingProject);
            _unitOfWork.Commit();

            response.Status = result.Item1 == EntityState.Modified ? ActionStatus.Success : ActionStatus.Failed;
            response.Entity = result.Item2;
            return response;
        }

        public async Task<CommonActionResult<Project>> DeleteProjectAsync(Guid projectId)
        {
            var response = new CommonActionResult<Project>();
            var projectRepository = _unitOfWork.GetRepository<Project>();

            var existingProject = await projectRepository.GetAsync(projectId);
            if (existingProject == null)
            {
                response.Status = ActionStatus.NotFound;
                return response;
            }

            var result = projectRepository.Delete(existingProject);
            _unitOfWork.Commit();

            response.Status = result == EntityState.Deleted ? ActionStatus.Success : ActionStatus.Failed;
            response.Entity = existingProject;
            return response;
        }

        public bool IsExists(Guid id)
        {
            var projectRepository = _unitOfWork.GetRepository<Project>();
            return projectRepository.Exists(project => project.Id == id);
        }
    }

}
