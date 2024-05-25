namespace IOTBackend.Domain.Dtos
{
    public class DeviceInstanceCreateDto
    {
        public Double XCordinate { get; set; }
        public Double YCordinate { get; set; }
        public Guid ProjectId { get; set; }
        public Guid DeviceId { get; set; }
    }


}
