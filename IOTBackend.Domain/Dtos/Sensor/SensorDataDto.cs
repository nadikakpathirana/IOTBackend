﻿namespace IOTBackend.Domain.Dtos
{
    public class SensorDataDto
    {
        public int Value { get; set; }


        public Guid APIKey { get; set; }

        public string DeviceId { get; set; }
    }
}
