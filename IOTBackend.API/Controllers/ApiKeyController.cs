using AutoMapper;
using Microsoft.AspNetCore.Mvc;

using IOTBackend.Application.Interfaces;
using IOTBackend.Domain.DbEntities;
using IOTBackend.Domain.Dtos;
using IOTBackend.Shared.Enums;
using IOTBackend.Shared.Responses;


namespace IOTBackend.API.Controllers
{
    [ApiController]
    [Route("api/apikeys/")]
    public class ApiKeyController : ControllerBase
    {
        private readonly IAPIKeyService _apiKeyService;

        public ApiKeyController(IAPIKeyService apiKeyService, IMapper mapper)
        {
            _apiKeyService = apiKeyService;
        }
        
        
        [HttpGet]
        public async Task<ActionResult<ApiRequestResult<ApiKey>>> GetAll()
        {
            var apiKeys = await _apiKeyService.GetAll();
        
            var response = new ApiRequestResult<List<ApiKey>>
            {
                Status = true,
                Message = "API Keys fetched successfully",
                Data = apiKeys
            };
        
            return Ok(response);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<ApiRequestResult<ApiKey>>> GetByUser(Guid userId)
        {
            var apiKey = await _apiKeyService.GetKeyOfAUser(userId);

            if (apiKey == null)
            {
                var errorResponse = new ApiRequestResult<ApiKey>
                {
                    Message = "API Key not found"
                };
                return NotFound(errorResponse);
            }

            var response = new ApiRequestResult<ApiKey>
            {
                Status = true,
                Message = "API Key fetched successfully",
                Data = apiKey
            };

            return Ok(response);
        }
        
        [HttpGet("{keyId}")]
        public async Task<ActionResult<ApiRequestResult<ApiKey>>> Get(Guid keyId)
        {
            var apiKey = await _apiKeyService.GetKey(keyId);

            if (apiKey == null)
            {
                var errorResponse = new ApiRequestResult<ApiKey>
                {
                    Message = "API Key not found"
                };
                return NotFound(errorResponse);
            }

            var response = new ApiRequestResult<ApiKey>
            {
                Status = true,
                Message = "API Key fetched successfully",
                Data = apiKey
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ApiRequestResult<ApiKey>>> Create(ApiKeyAddDto apiKeyAddDto)
        {
            var result = await _apiKeyService.CreateKey(apiKeyAddDto);

            if (result.Status == ActionStatus.Failed)
            {
                var errorResponse = new ApiRequestResult<ApiKey>
                {
                    Status = false,
                    Message = "Failed to create API Key"
                };
                return BadRequest(errorResponse);
            }

            var response = new ApiRequestResult<ApiKey>
            {
                Status = true,
                Message = "API Key created successfully",
                Data = result.Entity
            };

            return Created($"api/apikeys/{result?.Entity?.Id}", response);
        }

        [HttpPatch("{keyId}")]
        public async Task<ActionResult<ApiRequestResult<ApiKey>>> Update(Guid keyId, ApiKeyUpdateDto key)
        {
            var result = await _apiKeyService.UpdateKey(keyId, key);

            if (result.Status == ActionStatus.NotFound)
            {
                var errorResponse = new ApiRequestResult<ApiKey>
                {
                    Status = false,
                    Message = "API Key not found"
                };
                return NotFound(errorResponse);
            }

            if (result.Status == ActionStatus.Failed)
            {
                var errorResponse = new ApiRequestResult<ApiKey>
                {
                    Status = false,
                    Message = "Failed to update API Key"
                };
                return BadRequest(errorResponse);
            }

            var response = new ApiRequestResult<ApiKey>
            {
                Status = true,
                Message = "API Key updated successfully",
                Data = result.Entity
            };

            return Ok(response);
        }

        [HttpDelete("{keyId}")]
        public async Task<ActionResult<ApiRequestResult<ApiKey>>> Delete(Guid keyId)
        {
            var result = await _apiKeyService.DeleteKey(keyId);

            if (result.Status == ActionStatus.NotFound)
            {
                var errorResponse = new ApiRequestResult<ApiKey>
                {
                    Status = false,
                    Message = "API Key not found"
                };
                return NotFound(errorResponse);
            }

            if (result.Status == ActionStatus.Failed)
            {
                var errorResponse = new ApiRequestResult<ApiKey>
                {
                    Status = false,
                    Message = "Failed to delete API Key"
                };
                return BadRequest(errorResponse);
            }

            var response = new ApiRequestResult<ApiKey>
            {
                Status = true,
                Message = "API Key deleted successfully",
            };

            return Ok(response);
        }
    }
}
