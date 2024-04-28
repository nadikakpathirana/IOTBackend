using IOTBackend.Application.Interfaces;
using IOTBackend.Domain.DbEntities;
using IOTBackend.Infrastructure.Interfaces;
using IOTBackend.Persistance;
using IOTBackend.Shared.Enums;
using IOTBackend.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOTBackend.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork<AppDbContext> _unitOfWork;

        public UserService(IUnitOfWork<AppDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            try
            {
                var userRepository = _unitOfWork.GetRepository<User>();

                var data = userRepository.GetAll().ToList();

                return await Task.FromResult(data);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error with retrieving users", ex);
            }
        }

        public async Task<User> GetUserAsync(Guid userId)
        {
            try
            {
                var userRepository = _unitOfWork.GetRepository<User>();
                var entity = await userRepository.GetAsync(userId);
                return await Task.FromResult(entity);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error retrieving user with ID {userId}", ex);
            }
        }

        public async Task<CommonActionResult> AddUserAsync(User user)
        {
            try
            {
                var response = new CommonActionResult();

                var userRepository = _unitOfWork.GetRepository<User>();

                response = CheckDuplicateUser(user, response);

                if (response.Status == ActionStatus.Error)
                {
                    return response;
                }

                var result = await userRepository.AddAsync(user);
                _unitOfWork.Commit();

                response.Status = result == EntityState.Added ? ActionStatus.Success : ActionStatus.Failed;

                return response;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error creating user", ex);
            }
            
        }

        public async Task<CommonActionResult> UpdateUserAsync(Guid userId, User user)
        {
            try
            {
                var response = new CommonActionResult();

                var userRepository = _unitOfWork.GetRepository<User>();

                response = IsExists(user.Id, response);

                if (response.Status == ActionStatus.Failed)
                {
                    return await Task.FromResult(response);
                }

                var result = userRepository.Update(user);
                _unitOfWork.Commit();

                response.Status = result == EntityState.Modified ? ActionStatus.Success : ActionStatus.Failed;

                return await Task.FromResult(response);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error updating user with ID {userId}", ex);
            }
        }

        public async Task<CommonActionResult> DeleteUserAsync(Guid userId)
        {
            try
            {
                var response = new CommonActionResult();

                var userRepository = _unitOfWork.GetRepository<User>();

                response = IsExists(userId, response);

                if (response.Status == ActionStatus.Failed)
                {
                    response.Status = ActionStatus.NotFound;
                    response.Message = "User not found";

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
            catch (Exception ex)
            {
                throw new ApplicationException($"Error deleting user with ID {userId}", ex);
            }
        }

        private CommonActionResult CheckDuplicateUser(User user, CommonActionResult response)
        {
            response.Status = ActionStatus.Success;
            var userRepository = _unitOfWork.GetRepository<User>();

            var existUser = userRepository.FindBy(u => u.Email == user.Email && u.Id == user.Id).FirstOrDefault();

            if (existUser == null) return response;

            response.Status = ActionStatus.Error;
            response.Message = "User already exists";
            return response;
        }

        private CommonActionResult IsExists(Guid userId, CommonActionResult response)
        {
            response.Status = ActionStatus.Failed;
            var userRepository = _unitOfWork.GetRepository<User>();

            var existUser = userRepository.FindBy(u => u.Id == userId).FirstOrDefault();

            if (existUser == null) return response;

            response.Status = ActionStatus.Success;
            response.Message = "User exists";
            return response;
        }
    }
}
