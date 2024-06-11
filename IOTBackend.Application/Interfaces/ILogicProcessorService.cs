using IOTBackend.Domain.Dtos;

namespace IOTBackend.Application.Interfaces;

public interface ILogicProcessorService
{
    Dictionary<string, List<string>> GetDevicesWithPins(string json, SensorDataDto sensorData);
}