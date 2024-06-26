﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using IOTBackend.Domain.Dtos;
using IOTBackend.Shared.Responses;
using IOTBackend.Application.Interfaces;
using IOTBackend.Domain.DbEntities;

namespace IOTBackend.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public AuthController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiRequestResult<User>>> Register(UserRegisterDto userRegisterDto)
        {
            var result = await _authService.Register(userRegisterDto);

            if (!result.Status)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(UserLoginDto userLoginDto)
        {
            var user = await _authService.Login(userLoginDto);

            if (user == null)
            {
                ModelState.AddModelError("CustomeError", "Invalid credentials");
                return BadRequest(ModelState);
            }

            string token = _authService.GenerateToken(user);
            user.Token = token;

            return Ok(user);
        }

        [HttpGet, Authorize]
        public ActionResult<string> GetUserClaims()
        {
            return Ok(_userService.GetUserClaims());
        }
    }
}
