using IOTBackend.Application.Interfaces;
using System.Net.WebSockets;
using System.Text;

namespace IOTBackend.API.Controllers
{
    public class WebSocketController
    {
        private readonly WebSocket _webSocket;
        private readonly IWebSocketService _webSocketService;

        public WebSocketController(WebSocket webSocket, IWebSocketService webSocketService)
        {
            _webSocket = webSocket;
            _webSocketService = webSocketService;
        }

        public async Task HandleWebSocket(string type, Guid deviceId, Guid apiKey, Guid userId)
        {
            // should validate api key and userId
            
            // Add the connected client to the service
            _webSocketService.AddClient(type, deviceId, _webSocket);
            
            try
            {
                var buffer = new byte[1024 * 4];
                WebSocketReceiveResult result;

                while (_webSocket.State == WebSocketState.Open)
                {
                    result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), default);
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        // await _webSocketService.BroadcastMessage(message);
                        await _webSocketService.ProcessAndTransmit(message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                _webSocketService.RemoveClient(deviceId, _webSocket);
                await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed", default);
            }
        }
    }
}
