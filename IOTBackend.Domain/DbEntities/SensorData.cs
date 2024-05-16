using IOTBackend.Domain.DbEntities.BaseEntities;

namespace IOTBackend.Domain.DbEntities
{
    public class SensorData : ModelBase
    {
        public int Value { get; set; }

        public string ValueType { get; set; }

        public Guid APIKey { get; set; }

        public Device Device { get; set; }
        public Guid DeviceId { get; set; }
    }
}
