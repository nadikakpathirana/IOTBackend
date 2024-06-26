﻿using IOTBackend.Domain.DbEntities.BaseEntities;

namespace IOTBackend.Domain.DbEntities
{
    public class APIKey : ModelBase
    {
        public string Name { get; set; }
        public User Owner { get; set; }
        public Guid UserId { get; set; }
    }
}
