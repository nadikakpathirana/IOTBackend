namespace IOTBackend.Domain.Dtos
{
    public class ConnectionLineCreateDto
    {
        public Guid FromDevice { get; set; }

        public Guid ToDevice { get; set; }

        public string? Condition { get; set; }

        public Double StartXCordinate { get; set; }
        public Double StartYCordinate { get; set; }
        public Double EndXCordinate { get; set; }
        public Double EndYCordinate { get; set; }

        public Guid ProjectId { get; set; }
    }
}
