using IOTBackend.Application.Interfaces;
using IOTBackend.Domain.DbEntities;
using IOTBackend.Infrastructure.Interfaces;
using IOTBackend.Persistance;
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

        public IEnumerable<User> GetAllUsers()
        {
            var userRepository = _unitOfWork.GetRepository<User>();

            if (userRepository == null)
            {
                throw new InvalidOperationException("UserRepository not found");
            }

            return userRepository.GetAll();
        }
    }
}
