using IOTBackend.Application.Interfaces;
using IOTBackend.Application.Services;
using IOTBackend.Domain.DbEntities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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

        [HttpGet("")]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            var users = _userService.GetAllUsers();
            return Ok(users);
        }
    }
}
