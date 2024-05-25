namespace IOTBackend.Domain.Dtos
{
    public class SensorDataDto
    {
        public dynamic Value { get; set; }

        public string ValueType { get; set; }

        public Guid APIKey { get; set; }

        public Guid DeviceId { get; set; }
    }
}
