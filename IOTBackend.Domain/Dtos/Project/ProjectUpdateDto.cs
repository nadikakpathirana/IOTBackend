﻿using IOTBackend.Domain.DbEntities.BaseEntities;

namespace IOTBackend.Domain.Dtos
{
    public class ProjectUpdateDto : ModelBase
    {
        public string Name { get; set; }
    }
}