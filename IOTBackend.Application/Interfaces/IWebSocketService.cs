using System.Net.WebSockets;

namespace IOTBackend.Application.Interfaces
{
    public interface IWebSocketService
    {
        void AddClient(WebSocket webSocket);
        void RemoveClient(WebSocket webSocket);
        Task BroadcastMessage(string message);
    }
}
