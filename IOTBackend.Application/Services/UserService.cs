using IOTBackend.Application.Interfaces;
using IOTBackend.Domain.DbEntities;
using IOTBackend.Infrastructure.Interfaces;
using IOTBackend.Persistance;
using IOTBackend.Shared.Enums;
using IOTBackend.Shared.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Security.Claims;
using System.Text.Json;
using AutoMapper;
using IOTBackend.Domain.Dtos;
using Microsoft.IdentityModel.Tokens;


namespace IOTBackend.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork<AppDbContext> _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork<AppDbContext> unitOfWork, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            var userRepository = _unitOfWork.GetRepository<User>();

            var data = userRepository.GetAll().ToList();

            return await Task.FromResult(data);
        }

        public async Task<User?> GetUserAsync(string username)
        {
            var userRepository = _unitOfWork.GetRepository<User>();
            var entity = await userRepository.FindByAsync(u => u.Username == username);

            if (entity.IsNullOrEmpty())
            {
                return null;
            }
            return entity[0];
        }

        public async Task<CommonActionResult<User>> AddUserAsync(User user)
        {
            var response = new CommonActionResult<User>();

            var userRepository = _unitOfWork.GetRepository<User>();

            user.Id = new Guid();
            user.Created = DateTime.UtcNow;
            var result = await userRepository.AddAsync(user);
            _unitOfWork.Commit();

            response.Status = result.Item1 == EntityState.Added ? ActionStatus.Success : ActionStatus.Failed;
            response.Entity = result.Item2;

            return response;
        }

        public async Task<CommonActionResult<User>> UpdateUserAsync(Guid userId, UserUpdateDto user)
        {
            var response = new CommonActionResult<User>();

            var userRepository = _unitOfWork.GetRepository<User>();

            var existingUser = await userRepository.GetAsync(userId);

            if (existingUser == null)
            {
                response.Status = ActionStatus.NotFound;
                return response;
            }
            
            var newUser = _mapper.Map<User>(user);

            newUser.Id = existingUser.Id;
            newUser.Created = existingUser.Created;
            
            var result = userRepository.Update(newUser);

            try
            {
                _unitOfWork.Commit();

                response.Status = result.Item1 == EntityState.Modified ? ActionStatus.Success : ActionStatus.Failed;
            }
            catch (Exception ex)
            {
                var sqlException = ex.GetBaseException() as PostgresException;

                response.Status = ActionStatus.Error;


                if (sqlException != null)
                {
                    var code = sqlException.Code;

                    response.ErrorResult = new CommonErrorResultDto
                    {
                        customErrorCode = code,
                        exception = sqlException
                    };
                }
            }
            response.Entity = result.Item2;
            
            return await Task.FromResult(response);
        }

        public async Task<CommonActionResult<User>> DeleteUserAsync(Guid userId)
        {
            var response = new CommonActionResult<User>();

            var userRepository = _unitOfWork.GetRepository<User>();

            response = IsExists(userId, response);

            if (response.Status == ActionStatus.Failed)
            {
                response.Status = ActionStatus.NotFound;

                return response;
            }

            var entity = await userRepository.GetAsync(userId);
            var status = userRepository.Delete(entity);

            try
            {
                _unitOfWork.Commit();

                response.Status = status == EntityState.Deleted ? ActionStatus.Success : ActionStatus.Failed;
            }
            catch (Exception ex)
            {
                var sqlException = ex.GetBaseException() as PostgresException;

                response.Status = ActionStatus.Error;


                if (sqlException != null)
                {
                    var code = sqlException.Code;

                    response.ErrorResult = new CommonErrorResultDto
                    {
                        customErrorCode = code,
                        exception = sqlException
                    };
                }
            }
            return response;
        }

        private CommonActionResult<User> CheckDuplicateUser(User user, CommonActionResult<User> response)
        {
            response.Status = ActionStatus.Success;
            var userRepository = _unitOfWork.GetRepository<User>();

            var existUser = userRepository.FindBy(u => u.Email == user.Email && u.Id == user.Id).FirstOrDefault();

            if (existUser == null) return response;

            response.Status = ActionStatus.Error;
            return response;
        }

        private CommonActionResult<User> IsExists(Guid userId, CommonActionResult<User> response)
        {
            response.Status = ActionStatus.Failed;
            var userRepository = _unitOfWork.GetRepository<User>();

            var existUser = userRepository.FindBy(u => u.Id == userId).FirstOrDefault();

            if (existUser == null) return response;

            response.Status = ActionStatus.Success;
            return response;
        }

        public Dictionary<string,string> GetUserClaims()
        {

            var httpContext = _httpContextAccessor.HttpContext;

            var user = httpContext?.User;

            if (user != null && user.Identity is ClaimsIdentity claimsIdentity)
            {
                var claims = claimsIdentity.Claims;
                var claimDictionary = claims.ToDictionary(c => c.Type, c => c.Value);
                var json = JsonSerializer.Serialize(claimDictionary);

                return claimDictionary;
            }
            else
            {
                return new Dictionary<string, string>();
            }
        }


        public bool IsEmailExists(string email)
        {
            var userRepository = _unitOfWork.GetRepository<User>();

            var existUser = userRepository.FindBy(u => u.Email == email).FirstOrDefault();

            return existUser != null;
        }

        public bool IsUsernameExists(string username)
        {
            var userRepository = _unitOfWork.GetRepository<User>();

            var existUser = userRepository.FindBy(u => u.Username == username).FirstOrDefault();

            return existUser != null;
        }
    }
}
