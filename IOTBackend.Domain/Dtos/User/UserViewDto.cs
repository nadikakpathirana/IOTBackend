using IOTBackend.Domain.DbEntities.BaseEntities;
using IOTBackend.Shared.Enums;

namespace IOTBackend.Domain.Dtos
{
    public class UserViewDto: ModelBase
    {
        public string FName { get; set; } = string.Empty;
        public string LName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserType UserType { get; set; }

    }
}
