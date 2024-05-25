using Microsoft.AspNetCore.Mvc;
using IOTBackend.Application.Interfaces;
using IOTBackend.Domain.DbEntities;
using IOTBackend.Domain.Dtos;
using IOTBackend.Shared.Enums;
using IOTBackend.Shared.Responses;

namespace IOTBackend.API.Controllers
{
    [ApiController]
    [Route("api/connectionlines/")]
    public class ConnectionLineController : ControllerBase
    {
        private readonly IConnectionLineService _connectionLineService;

        public ConnectionLineController(IConnectionLineService connectionLineService)
        {
            _connectionLineService = connectionLineService;
        }
        
        [HttpGet]
        public async Task<ActionResult<ApiRequestResult<List<ConnectionLine>>>> GetAllConnectionLines()
        {
            var connectionLines = await _connectionLineService.GetAll();

            var response = new ApiRequestResult<List<ConnectionLine>>
            {
                Status = true,
                Message = "Connection lines fetched successfully",
                Data = connectionLines
            };

            return Ok(response);
        }

        [HttpGet("project/{projectId}")]
        public async Task<ActionResult<ApiRequestResult<List<ConnectionLine>>>> GetConnectionLinesOfProject(Guid projectId)
        {
            var connectionLines = await _connectionLineService.GetConnectionLinesOfProjectAsync(projectId);

            var response = new ApiRequestResult<List<ConnectionLine>>
            {
                Status = true,
                Message = "Connection lines fetched successfully",
                Data = connectionLines
            };

            return Ok(response);
        }

        [HttpGet("{connectionLineId}")]
        public async Task<ActionResult<ApiRequestResult<ConnectionLine>>> Get(Guid connectionLineId)
        {

            var connectionLine = await _connectionLineService.GetConnectionLineAsync(connectionLineId);

            if (connectionLine == null)
            {
                var errorResponse = new ApiRequestResult<List<ConnectionLine>>
                {
                    Message = "Connection line not found"
                };
                return NotFound(errorResponse);
            }

            var response = new ApiRequestResult<ConnectionLine>
            {
                Status = true,
                Message = "Connection line fetched successfully",
                Data = connectionLine
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ApiRequestResult<ConnectionLine>>> Create(ConnectionLineCreateDto connectionLine)
        {
            var result = await _connectionLineService.CreateConnectionLineAsync(connectionLine);

            if (result.Status == ActionStatus.Error)
            {
                var errorResponse = new ApiRequestResult<ConnectionLine>
                {
                    Status = false,
                    Message = "Failed to create connection line"
                };
                return BadRequest(errorResponse);
            }

            var response = new ApiRequestResult<ConnectionLine>
            {
                Status = true,
                Message = "Connection line created successfully",
                Data = result.Entity
            };

            return Created($"GetConnectionLine/{result?.Entity?.Id}", response);
        }

        [HttpPatch("{connectionLineId}")]
        public async Task<ActionResult<ApiRequestResult<ConnectionLine>>> Update(Guid connectionLineId, ConnectionLineUpdateDto connectionLine)
        {
            var result = await _connectionLineService.UpdateConnectionLineAsync(connectionLineId, connectionLine);

            if (result.Status == ActionStatus.NotFound)
            {
                var errorResponse = new ApiRequestResult<ConnectionLine>
                {
                    Status = false,
                    Message = "Connection line not found"
                };
                return NotFound(errorResponse);
            }

            if (result.Status == ActionStatus.Failed)
            {
                var errorResponse = new ApiRequestResult<ConnectionLine>
                {
                    Status = false,
                    Message = "Failed to update connection line"
                };
                return BadRequest(errorResponse);
            }

            var response = new ApiRequestResult<ConnectionLine>
            {
                Status = true,
                Message = "Connection line updated successfully",
                Data = result.Entity
            };

            return Ok(response);
        }

        [HttpDelete("{connectionLineId}")]
        public async Task<ActionResult<ApiRequestResult<ConnectionLine>>> Delete(Guid connectionLineId)
        {
            var result = await _connectionLineService.DeleteConnectionLineAsync(connectionLineId);

            if (result.Status == ActionStatus.NotFound)
            {
                var errorResponse = new ApiRequestResult<ConnectionLine>
                {
                    Status = false,
                    Message = "Connection line not found"
                };
                return NotFound(errorResponse);
            }

            if (result.Status == ActionStatus.Failed)
            {
                var errorResponse = new ApiRequestResult<ConnectionLine>
                {
                    Status = false,
                    Message = "Failed to delete connection line"
                };
                return BadRequest(errorResponse);
            }

            var response = new ApiRequestResult<ConnectionLine>
            {
                Status = true,
                Message = "Connection line deleted successfully",
                Data = result.Entity
            };

            return Ok(response);
        }
    }
}
