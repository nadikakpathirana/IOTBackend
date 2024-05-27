using IOTBackend.Application.Interfaces;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using AutoMapper.Internal;
using IOTBackend.Domain.DbEntities;
using IOTBackend.Domain.Dtos;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace IOTBackend.Application.Services
{
    public class WebSocketService : IWebSocketService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogicProcessorService _logicProcessorService;
        
        private readonly ConcurrentDictionary<Guid, WebSocket> _connectedSensors = new ();
        private readonly ConcurrentDictionary<Guid, WebSocket> _connectedOutputs = new ();
        private readonly ConcurrentDictionary<Guid, WebSocket> _connectedUis = new ();

        private ConcurrentDictionary<Guid, string> _type = new ();


        public WebSocketService(IServiceProvider serviceProvider, ILogicProcessorService logicProcessorService)
        {
            _serviceProvider = serviceProvider;
            _logicProcessorService = logicProcessorService;
        }
        
        public void AddClient(string type, Guid deviceId, WebSocket webSocket)
        {
            switch (type)
            {
                case "input":
                    _connectedSensors.TryAdd(deviceId, webSocket);
                    _type.TryAdd(deviceId, type);
                    Console.WriteLine("input connected");
                    break;
                case "output":
                    _connectedOutputs.TryAdd(deviceId, webSocket);
                    _type.TryAdd(deviceId, type);
                    Console.WriteLine("output connected");
                    break;
                case "ui":
                    _connectedUis.TryAdd(deviceId, webSocket);
                    _type.TryAdd(deviceId, type);
                    Console.WriteLine("ui connected");
                    break;
                default:
                    break;
            }
            
        }

        public void RemoveClient(Guid deviceId, WebSocket webSocket)
        {
            try
            {
                if (_type.TryGetValue(deviceId, out string? guid))
                {
                    _type.TryRemove(deviceId, out _);
                    Console.WriteLine("Removed from the _type");
                }
            }
            catch (Exception e)
            {
                // Console.WriteLine(e);
            }
            try
            {
                _connectedSensors.TryRemove(deviceId, out _);
                Console.WriteLine("Removed from the _connectedSensors");
                
            }
            catch (Exception e)
            {
                // Console.WriteLine(e);
            }
            
            try
            {
                _connectedOutputs.TryRemove(deviceId, out _);
                Console.WriteLine("Removed from the _connectedOutputs");
            }
            catch (Exception e)
            {
                // Console.WriteLine(e);
            }
            
            try
            {
                _connectedUis.TryRemove(deviceId, out _);
                Console.WriteLine("Removed from the _connectedUis");
            }
            catch (Exception e)
            {
                // Console.WriteLine(e);
            }
        }

        public async Task BroadcastMessage(string message)
        {
            foreach (var client in _connectedOutputs.Values)
            {
                if (client.State == WebSocketState.Open)
                {
                    await client.SendAsync(Encoding.UTF8.GetBytes(message), WebSocketMessageType.Text, true, default);
                }
            }
        }
        
        public async Task ProcessAndTransmit(string msg)
        {
            // map json to a object
            var sensorData = JsonConvert.DeserializeObject<SensorDataDto>(msg);

            if (sensorData != null)
            {
                
                // should get all connections line including above id as fromDevice
                var connectionLines = await GetConnectionLines(sensorData.DeviceId);
            
                // check walid connection lines
                List<ConnectionLine> validConnectionLines = new List<ConnectionLine>();
            
                // get ToDevices of the valid connectioon lines
                foreach (var connectionLine in connectionLines)
                {
                    if (true)
                    {
                        validConnectionLines.Add(connectionLine);
                    }
                }
                
                // get output devices of the valid connection lines
                
                List<Guid> validDeviceInstanceIds = new List<Guid>();
                foreach (var validConnectionLine in validConnectionLines)
                {
                    if (validConnectionLine.Condition == null && await _logicProcessorService.Process(sensorData.Value, validConnectionLine.Condition))
                    {
                        validDeviceInstanceIds.TryAdd(validConnectionLine.ToDevice);
                    }
                }
                
                // define messages for Uis and Output Devices
                
                // define messages for Output Devices and Output Devices
                
            
                // transmit the message in to them
                
                var transmitToOutputsTask = TransmitToOutputs("Ring Weyaan", validDeviceInstanceIds);
                var transmitToUisTask = TransmitToUis("UI eke pennapaan");
            
                await Task.WhenAll(transmitToUisTask, transmitToOutputsTask);
            }
        }

        private async Task<bool> TransmitToUis(string msg)
        {
            foreach (var client in _connectedUis.Values)
            {
                if (client.State == WebSocketState.Open)
                {
                    await client.SendAsync(Encoding.UTF8.GetBytes(msg), WebSocketMessageType.Text, true, default);
                }
            }
            return true;
        }
        
        private async Task<bool> TransmitToOutputs(string msg, List<Guid> validDeviceInstanceIds)
        {
            foreach (var (deviceId, client) in _connectedOutputs)
            {
                if (validDeviceInstanceIds.Contains(deviceId) && client.State == WebSocketState.Open)
                {
                    await client.SendAsync(Encoding.UTF8.GetBytes(msg), WebSocketMessageType.Text, true, default);
                }
            }
            return true;
        }

        private async Task<List<ConnectionLine>> GetConnectionLines(Guid deviceId)
        {
            using var scope = _serviceProvider.CreateScope();
            var connectionLineService = scope.ServiceProvider.GetRequiredService<IConnectionLineService>();
            return await connectionLineService.GetConnectionLinesOfDevicesBeginWith(deviceId);
        }
        
    }
}
