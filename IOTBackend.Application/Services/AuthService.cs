using AutoMapper;
using IOTBackend.Application.Interfaces;
using IOTBackend.Domain.DbEntities;
using IOTBackend.Domain.Dtos;
using IOTBackend.Shared.Enums;
using IOTBackend.Shared.Responses;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IOTBackend.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        
        public AuthService(IMapper mapper, IUserService userService, IConfiguration configuration)
        {
            _mapper = mapper;
            _userService = userService;
            _configuration = configuration;

        }

        public async Task<ApiRequestResult<User>> Register(UserRegisterDto userRegisterDto)
        {
            var result = new ApiRequestResult<User>();

            if (_userService.IsEmailExists(userRegisterDto.Email))
            {
                result.Status = false;
                result.Message = "Email already exists";
                return result;
            }

            if (_userService.IsUsernameExists(userRegisterDto.Username))
            {
                result.Status = false;
                result.Message = "Username already exists";
                return result;
            }

            var newUser = _mapper.Map<User>(userRegisterDto);

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(userRegisterDto.Password);

            newUser.PasswordHash = passwordHash;

            var addResult = await _userService.AddUserAsync(newUser);

            if (addResult.Status == ActionStatus.Error)
            {
                result.Status = false;
                result.Message = "Failed to add user";
                return result;
            }

            result.Status = true;
            result.Message = "User added successfully";
            result.Data = addResult.Entity;

            return result;
        }

        public async Task<User?> Login(UserLoginDto userLoginDto)
        {
            var user = await _userService.GetUserAsync(userLoginDto.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(userLoginDto.Password, user.PasswordHash))
            {
                return null;
            }
            return user;
        }

        public string GenerateToken(User user)
        {
            List<Claim> claims = new()
        {
            new Claim("Id", user.Id.ToString()),
            new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
            new Claim("Email", user.Email ?? ""),
            new Claim("Username", user.Username ?? "")
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
