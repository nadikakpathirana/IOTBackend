using IOTBackend.Application.Interfaces;
using IOTBackend.Application.Services;
using IOTBackend.Domain.DbEntities;
using IOTBackend.Shared.Enums;
using IOTBackend.Shared.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using IOTBackend.Domain.Dtos;

namespace IOTBackend.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();

            var response = new ApiRequestResult<List<User>>
            {
                Status = true,
                Message = "Users fetched successfully",
                Data = users
            };

            return Ok(response);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<ApiRequestResult<User>>> GetUser(string username)
        {

            var user = await _userService.GetUserAsync(username);

            if (user == null)
            {
                var errorResponse = new ApiRequestResult<List<User>>
                {
                    Message = "User not found"
                };
                return NotFound(errorResponse);
            }

            var response = new ApiRequestResult<User>
            {
                Status = true,
                Message = "User fetched succuessfully",
                Data = user
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ApiRequestResult<User>>> AddUser(User user)
        {

            var result = await _userService.AddUserAsync(user);

            if (result.Status == ActionStatus.Error)
            {
                var errorResponse = new ApiRequestResult<User>
                {
                    Status = false,
                    Message = "Failed to add user"
                };
                return BadRequest(errorResponse);
            }

            var response = new ApiRequestResult<User>
            {
                Status = true,
                Message = "User added successfully",
                Data = user
            };

            return Created($"GetUser", response);
        }

        [HttpPut("{userId}")]
        public async Task<ActionResult<ApiRequestResult<User>>> UpdateUser(Guid userId, UserUpdateDto user)
        {
            var result = await _userService.UpdateUserAsync(userId, user);

            if (result.Status == ActionStatus.Error)
            {
                var errorResponse = new ApiRequestResult<User>
                {
                    Status = false,
                    Message = "Failed to update user"
                };
                return BadRequest(errorResponse);
            }

            var response = new ApiRequestResult<User>
            {
                Status = true,
                Message = "User updated successfully",
                Data = result.Entity
            };

            return Ok(response);
        }

        [HttpDelete("{userId}")]
        public async Task<ActionResult<ApiRequestResult<User>>> DeleteUser(Guid userId)
        {
            var result = await _userService.DeleteUserAsync(userId);

            if (result.Status == ActionStatus.Failed)
            {
                var errorResponse = new ApiRequestResult<User>
                {
                    Status = false,
                    Message = "Failed to delete user"
                };
                return BadRequest(errorResponse);
            }

            var response = new ApiRequestResult<User>
            {
                Status = true,
                Message = "User deleted successfully"
            };


            return Ok(response);
        }
    }
}
