using AutoMapper;
using IOTBackend.Application.Interfaces;
using IOTBackend.Domain.DbEntities;
using IOTBackend.Domain.Dtos;
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
        private readonly IMapper _mapper;

        public ProjectService(IUnitOfWork<AppDbContext> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper;
        }
        
        public async Task<List<Project>> GetAll()
        {
            var projectRepository = _unitOfWork.GetRepository<Project>();
            var projects = await projectRepository.GetAll().ToListAsync();
            return projects;
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

        public async Task<CommonActionResult<Project>> CreateProjectAsync(ProjectCreateDto project)
        {
            var response = new CommonActionResult<Project>();
            var projectRepository = _unitOfWork.GetRepository<Project>();

            var newProject = _mapper.Map<Project>(project);
            
            newProject.Id = new Guid();
            newProject.Created = DateTime.UtcNow;
            var result = await projectRepository.AddAsync(newProject);
            _unitOfWork.Commit();

            response.Status = result.Item1 == EntityState.Added ? ActionStatus.Success : ActionStatus.Failed;
            response.Entity = result.Item2;
            return response;
        }

        public async Task<CommonActionResult<Project>> UpdateProjectAsync(Guid projectId, ProjectUpdateDto project)
        {
            var response = new CommonActionResult<Project>();
            var projectRepository = _unitOfWork.GetRepository<Project>();

            var existingProject = await projectRepository.GetAsync(projectId);
            if (existingProject == null)
            {
                response.Status = ActionStatus.NotFound;
                return response;
            }
            
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
