using System;
using System.Collections.Generic;

namespace IOTBackend.Domain.DbEntities.Aws
{
    public partial class Connection
    {
        public string Id { get; set; } = null!;
        public string Data { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
