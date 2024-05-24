using IOTBackend.Domain.DbEntities.BaseEntities;
using IOTBackend.Shared.Enums;

namespace IOTBackend.Domain.DbEntities
{
    public class DeviceUpdateDto : ModelBase
    {
        public string Name { get; set; }
        public DeviceType DeviceType { get; set; }
    }
}
