using IOTBackend.Domain.DbEntities.BaseEntities;

namespace IOTBackend.Domain.Dtos
{
    public class ConnectionLineUpdateDto : ModelBase
    {
        public Guid FromDevice { get; set; }

        public Guid ToDevice { get; set; }

        public string? Condition { get; set; }

        public Double StartXCordinate { get; set; }
        public Double StartYCordinate { get; set; }
        public Double EndXCordinate { get; set; }
        public Double EndYCordinate { get; set; }
    }
}
