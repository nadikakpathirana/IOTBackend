using IOTBackend.Application.Interfaces;
using IOTBackend.Domain.DbEntities;
using IOTBackend.Shared.Enums;
using IOTBackend.Shared.Responses;
using Microsoft.AspNetCore.Mvc;

namespace IOTBackend.API.Controllers
{
    [ApiController]
    [Route("api/projects")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<List<Project>>> GetAllProjectsOfUser(Guid userId)
        {
            var projects = await _projectService.GetAllProjectsOfUserAsync(userId);

            var response = new ApiRequestResult<List<Project>>
            {
                Status = true,
                Message = "Projects fetched successfully",
                Data = projects
            };

            return Ok(response);
        }

        [HttpGet("{projectId}")]
        public async Task<ActionResult<ApiRequestResult<Project>>> GetProject(Guid projectId)
        {
            var project = await _projectService.GetProjectAsync(projectId);

            if (project == null || project.Count == 0)
            {
                var errorResponse = new ApiRequestResult<Project>
                {
                    Status = false,
                    Message = "Project not found"
                };
                return NotFound(errorResponse);
            }

            var response = new ApiRequestResult<Project>
            {
                Status = true,
                Message = "Project fetched successfully",
                Data = project[0]
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ApiRequestResult<Project>>> CreateProject(Project project)
        {
            var result = await _projectService.CreateProjectAsync(project);

            if (result.Status == ActionStatus.Failed)
            {
                var errorResponse = new ApiRequestResult<Project>
                {
                    Status = false,
                    Message = "Failed to create project"
                };
                return BadRequest(errorResponse);
            }

            var response = new ApiRequestResult<Project>
            {
                Status = true,
                Message = "Project created successfully",
                Data = project
            };

            return Created($"GetProject/{result.Entity.Id}", response);
        }

        [HttpPut("{projectId}")]
        public async Task<ActionResult<ApiRequestResult<Project>>> UpdateProject(Guid projectId, Project project)
        {
            project.Id = projectId;
            var result = await _projectService.UpdateProjectAsync(project);

            if (result.Status == ActionStatus.NotFound)
            {
                var errorResponse = new ApiRequestResult<Project>
                {
                    Status = false,
                    Message = "Project not found"
                };
                return NotFound(errorResponse);
            }

            if (result.Status == ActionStatus.Failed)
            {
                var errorResponse = new ApiRequestResult<Project>
                {
                    Status = false,
                    Message = "Failed to update project"
                };
                return BadRequest(errorResponse);
            }

            var response = new ApiRequestResult<Project>
            {
                Status = true,
                Message = "Project updated successfully",
                Data = project
            };

            return Ok(response);
        }

        [HttpDelete("{projectId}")]
        public async Task<ActionResult<ApiRequestResult<Project>>> DeleteProject(Guid projectId)
        {
            var result = await _projectService.DeleteProjectAsync(projectId);

            if (result.Status == ActionStatus.NotFound)
            {
                var errorResponse = new ApiRequestResult<Project>
                {
                    Status = false,
                    Message = "Project not found"
                };
                return NotFound(errorResponse);
            }

            if (result.Status == ActionStatus.Failed)
            {
                var errorResponse = new ApiRequestResult<Project>
                {
                    Status = false,
                    Message = "Failed to delete project"
                };
                return BadRequest(errorResponse);
            }

            var response = new ApiRequestResult<Project>
            {
                Status = true,
                Message = "Project deleted successfully",
                Data = result.Entity
            };

            return Ok(response);
        }
    }
}
