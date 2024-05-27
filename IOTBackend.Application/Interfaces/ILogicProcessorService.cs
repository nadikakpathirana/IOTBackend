namespace IOTBackend.Application.Interfaces;

public interface ILogicProcessorService
{
    Task<bool> Process(int sensorValue, string condition);
}