using IOTBackend.Domain.DbEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOTBackend.Application.Interfaces
{
    public interface IUserService
    {
        IEnumerable<User> GetAllUsers();
    }
}
