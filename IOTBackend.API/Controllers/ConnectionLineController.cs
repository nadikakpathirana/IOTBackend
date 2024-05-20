using Microsoft.AspNetCore.Mvc;
using IOTBackend.Application.Interfaces;
using IOTBackend.Domain.DbEntities;
using IOTBackend.Shared.Enums;
using IOTBackend.Shared.Responses;

namespace IOTBackend.API.Controllers
{
    [ApiController]
    [Route("api/connectionlines")]
    public class ConnectionLineController : ControllerBase
    {
        private readonly IConnectionLineService _connectionLineService;

        public ConnectionLineController(IConnectionLineService connectionLineService)
        {
            _connectionLineService = connectionLineService;
        }

        [HttpGet("project/{projectId}")]
        public async Task<ActionResult<List<ConnectionLine>>> GetConnectionLinesOfProject(Guid projectId)
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
        public async Task<ActionResult<ApiRequestResult<ConnectionLine>>> GetConnectionLine(Guid connectionLineId)
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
        public async Task<ActionResult<ApiRequestResult<ConnectionLine>>> CreateConnectionLine(ConnectionLine connectionLine)
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
                Data = connectionLine
            };

            return Created($"GetConnectionLine/{result.Entity.Id}", response);
        }

        [HttpPut("{connectionLineId}")]
        public async Task<ActionResult<ApiRequestResult<ConnectionLine>>> UpdateConnectionLine(Guid connectionLineId, ConnectionLine connectionLine)
        {
            var result = await _connectionLineService.UpdateConnectionLineAsync(connectionLine);

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
                Data = connectionLine
            };

            return Ok(response);
        }

        [HttpDelete("{connectionLineId}")]
        public async Task<ActionResult<ApiRequestResult<ConnectionLine>>> DeleteConnectionLine(Guid connectionLineId)
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
