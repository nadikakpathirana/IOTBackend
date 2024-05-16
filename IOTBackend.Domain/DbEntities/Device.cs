using IOTBackend.Domain.DbEntities.BaseEntities;
using IOTBackend.Shared.Enums;

namespace IOTBackend.Domain.DbEntities
{
    public class Device : ModelBase
    {
        public string Name { get; set; }
        public DeviceType DeviceType { get; set; }

        public User? User { get; set; }
        public Guid UserId { get; set; }
    }
}
