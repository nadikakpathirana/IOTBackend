using IOTBackend.Domain.DbEntities.BaseEntities;

namespace IOTBackend.Domain.DbEntities
{
    public class DeviceInstance : ModelBase
    {
        public Double XCordinate { get; set; }
        public Double YCordinate { get; set; }

        public Project Project { get; set; }
        public Guid ProjectId { get; set; }
        
        public Device? Device { get; set; }
        public Guid DeviceId { get; set; }
    }


}
