using IOTBackend.Domain.DbEntities.BaseEntities;

namespace IOTBackend.Domain.Dtos
{
    public class ProjectCreateDto : ModelBase
    {
        public string Name { get; set; }
        public Guid UserId { get; set; }

    }
}
