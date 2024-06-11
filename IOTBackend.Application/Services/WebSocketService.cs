
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

using IOTBackend.Application.Interfaces;
using IOTBackend.Application.Services.Interfaces;
using IOTBackend.Domain.DbEntities;
using IOTBackend.Domain.Dtos;


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
                string json = @"{
                  'dd9f50f8-9efc-4a55-af74-514014c28a9b': {
                    '0': [
                      {
                        '0': 'val',
                        '1': '=',
                        '2': '10',
                        '11': {
                          '0': [
                            '2d875c70-a1d5-400e-834b-561fe8e12613'
                          ]
                        }
                      },
                      {
                        '0': 'val',
                        '1': '>',
                        '2': '50',
                        '10': [
                          {
                            '0': 'val',
                            '1': '>',
                            '2': '60',
                            '11': {
                              '0': [
                                '6ab7f049-a905-4374-9370-5cd6d24bd3b6'
                              ]
                            }
                          }
                        ],
                        '11': {
                            '0': [
                                '6ab7f049-a905-4374-9370-5cd6d24bd3b7'
                             ]
                        }
                      }
                    ]
                  }
                }";
                
                using var scope = _serviceProvider.CreateScope();
                var awsConnectionService = scope.ServiceProvider.GetRequiredService<IAwsConnectionService>();

                var connectionList = await awsConnectionService.GetAll();

                var connection = connectionList.First(c => c.Id == sensorData.DeviceId);
                
                var devicesWithPins = _logicProcessorService.GetDevicesWithPins(connection.Data, sensorData);
                
                Console.WriteLine(JsonConvert.SerializeObject(devicesWithPins, Formatting.Indented));
                
                foreach (var pins  in devicesWithPins)
                {
                    foreach (var deviceId in pins.Value)
                    {
                        var hasConnected = _connectedOutputs.ContainsKey(new Guid(deviceId));

                        if (hasConnected)
                        {
                            var client = _connectedOutputs[new Guid(deviceId)];
                    
                            if (client.State == WebSocketState.Open)
                            {
                                var message = new OutputObject("test");

                                // Serialize the object to a JSON string
                                string jsonString = JsonConvert.SerializeObject(message);

                                // Convert the JSON string to bytes
                                byte[] bytesToSend = Encoding.UTF8.GetBytes(jsonString);

                                // Send the bytes using WebSocket
                                await client.SendAsync(new ArraySegment<byte>(bytesToSend), WebSocketMessageType.Text, true, default);
                            }
                        }
                    }
                }
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

    public class OutputObject
    {
        private string _msg { get; set; }

        public OutputObject(string msg)
        {
            _msg = msg;
        }
    }
}
