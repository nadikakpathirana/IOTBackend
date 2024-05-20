using AutoMapper;
using Microsoft.AspNetCore.Mvc;

using IOTBackend.Application.Interfaces;
using IOTBackend.Domain.DbEntities;
using IOTBackend.Shared.Enums;
using IOTBackend.Shared.Responses;


namespace IOTBackend.API.Controllers
{
    [ApiController]
    [Route("api/apikeys/")]
    public class APIKeyController : ControllerBase
    {
        private readonly IAPIKeyService _apiKeyService;
        private readonly IMapper _mapper;

        public APIKeyController(IAPIKeyService apiKeyService, IMapper mapper)
        {
            _apiKeyService = apiKeyService;
            _mapper = mapper;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<ApiRequestResult<APIKey>>> GetByUser(Guid userId)
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
        
        [HttpGet("{keyId}")]
        public async Task<ActionResult<ApiRequestResult<APIKey>>> Get(Guid keyId)
        {
            var apiKey = await _apiKeyService.GetKeyOfAUser(keyId);

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
        public async Task<ActionResult<ApiRequestResult<APIKey>>> Create(APIKeyAddDto apiKeyAddDto)
        {
            var newKey = _mapper.Map<APIKey>(apiKeyAddDto);
            var result = await _apiKeyService.CreateKey(newKey);

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

            return Created($"GetKey", response);
        }

        [HttpPatch("{keyId}")]
        public async Task<ActionResult<ApiRequestResult<APIKey>>> Update(Guid keyId, APIKeyUpdateDto key)
        {
            key.Id = keyId;

            var updatedApiKey = _mapper.Map<APIKey>(key);
            updatedApiKey.Id = keyId;
            var result = await _apiKeyService.UpdateKey(updatedApiKey);

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
        public async Task<ActionResult<ApiRequestResult<APIKey>>> Delete(Guid keyId)
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
            };

            return Ok(response);
        }
    }
}
