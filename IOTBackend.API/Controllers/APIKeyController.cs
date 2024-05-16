using Microsoft.AspNetCore.Mvc;

using IOTBackend.Application.Interfaces;
using IOTBackend.Domain.DbEntities;
using IOTBackend.Shared.Enums;
using IOTBackend.Shared.Responses;


namespace IOTBackend.API.Controllers
{
    [ApiController]
    [Route("api/apikeys")]
    public class APIKeyController : ControllerBase
    {
        private readonly IAPIKeyService _apiKeyService;

        public APIKeyController(IAPIKeyService apiKeyService)
        {
            _apiKeyService = apiKeyService;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<ApiRequestResult<APIKey>>> GetKey(Guid userId)
        {
            var apiKey = await _apiKeyService.GetKeyOfAUser(userId);

            if (apiKey == null)
            {
                var errorResponse = new ApiRequestResult<APIKey>
                {
                    Message = "API Key not found"
                };
                return NotFound(errorResponse);
            }

            var response = new ApiRequestResult<APIKey>
            {
                Status = true,
                Message = "API Key fetched successfully",
                Data = apiKey
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ApiRequestResult<APIKey>>> CreateKey(APIKey key)
        {
            var result = await _apiKeyService.CreateKey(key);

            if (result.Status == ActionStatus.Failed)
            {
                var errorResponse = new ApiRequestResult<APIKey>
                {
                    Status = false,
                    Message = "Failed to create API Key"
                };
                return BadRequest(errorResponse);
            }

            var response = new ApiRequestResult<APIKey>
            {
                Status = true,
                Message = "API Key created successfully",
                Data = result.Entity
            };

            return Created($"GetKey/{result.Entity.Id}", response);
        }

        [HttpPut("{keyId}")]
        public async Task<ActionResult<ApiRequestResult<APIKey>>> UpdateKey(Guid keyId, APIKey key)
        {
            key.Id = keyId; // Ensure the key ID matches the route parameter
            var result = await _apiKeyService.UpdateKey(key);

            if (result.Status == ActionStatus.NotFound)
            {
                var errorResponse = new ApiRequestResult<APIKey>
                {
                    Status = false,
                    Message = "API Key not found"
                };
                return NotFound(errorResponse);
            }

            if (result.Status == ActionStatus.Failed)
            {
                var errorResponse = new ApiRequestResult<APIKey>
                {
                    Status = false,
                    Message = "Failed to update API Key"
                };
                return BadRequest(errorResponse);
            }

            var response = new ApiRequestResult<APIKey>
            {
                Status = true,
                Message = "API Key updated successfully",
                Data = result.Entity
            };

            return Ok(response);
        }

        [HttpDelete("{keyId}")]
        public async Task<ActionResult<ApiRequestResult<APIKey>>> DeleteKey(Guid keyId)
        {
            var result = await _apiKeyService.DeleteKey(keyId);

            if (result.Status == ActionStatus.NotFound)
            {
                var errorResponse = new ApiRequestResult<APIKey>
                {
                    Status = false,
                    Message = "API Key not found"
                };
                return NotFound(errorResponse);
            }

            if (result.Status == ActionStatus.Failed)
            {
                var errorResponse = new ApiRequestResult<APIKey>
                {
                    Status = false,
                    Message = "Failed to delete API Key"
                };
                return BadRequest(errorResponse);
            }

            var response = new ApiRequestResult<APIKey>
            {
                Status = true,
                Message = "API Key deleted successfully",
                Data = result.Entity
            };

            return Ok(response);
        }
    }
}
