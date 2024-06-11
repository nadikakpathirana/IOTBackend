using System;
using System.Collections.Generic;

namespace IOTBackend.Domain.DbEntities.Aws
{
    public partial class Device
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public int Type { get; set; }
        public int Status { get; set; }
        public bool? IsActive { get; set; }
        public string Location { get; set; } = null!;
        public string? Description { get; set; }
        public string VirtualPins { get; set; } = null!;
        public DateTime? LastCheck { get; set; }
        public Guid UserId { get; set; }
        public Guid? ProjectId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? ConnectionId { get; set; }

        public virtual Connection? Connection { get; set; }
        public virtual Project? Project { get; set; }
        public virtual User User { get; set; } = null!;
    }
}
