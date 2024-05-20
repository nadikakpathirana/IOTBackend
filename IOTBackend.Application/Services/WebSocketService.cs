using IOTBackend.Application.Interfaces;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

namespace IOTBackend.Application.Services
{
    public class WebSocketService : IWebSocketService
    {
        private readonly ConcurrentDictionary<string, WebSocket> _connectedClients = new ConcurrentDictionary<string, WebSocket>();

        public void AddClient(WebSocket webSocket)
        {
            _connectedClients.TryAdd(Guid.NewGuid().ToString(), webSocket);
        }

        public void RemoveClient(WebSocket webSocket)
        {
            _connectedClients.TryRemove(webSocket.GetHashCode().ToString(), out _);
        }

        public async Task BroadcastMessage(string message)
        {
            foreach (var client in _connectedClients.Values)
            {
                if (client.State == WebSocketState.Open)
                {
                    await client.SendAsync(Encoding.UTF8.GetBytes(message), WebSocketMessageType.Text, true, default);
                }
            }
        }
    }
}
