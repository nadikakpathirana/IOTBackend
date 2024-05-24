using IOTBackend.Domain.DbEntities.BaseEntities;

namespace IOTBackend.Domain.Dtos
{
    public class ApiKeyAddDto : ModelBase
    {
        public string Name { get; set; }
        public Guid UserId { get; set; }
    }
}
