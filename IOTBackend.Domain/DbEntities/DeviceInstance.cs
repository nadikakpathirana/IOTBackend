namespace IOTBackend.Domain.DbEntities
{
    public class DeviceInstance : Device
    {
        public Double XCordinate { get; set; }
        public Double YCordinate { get; set; }

        public Project Project { get; set; }
        public Guid ProjectId { get; set; }
    }


}
