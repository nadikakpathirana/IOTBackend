using IOTBackend.Domain.DbEntities.BaseEntities;

namespace IOTBackend.Domain.DbEntities
{
    public class APIKeyUpdateDto : ModelBase
    {
        public string Name { get; set; }
        public Guid UserId { get; set; }
    }
}
