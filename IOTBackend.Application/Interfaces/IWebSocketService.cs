using System.Net.WebSockets;

namespace IOTBackend.Application.Interfaces
{
    public interface IWebSocketService
    {
        void AddClient(string type, Guid deviceId, WebSocket webSocket);
        void RemoveClient(Guid deviceId, WebSocket webSocket);
        Task BroadcastMessage(string message);
        Task ProcessAndTransmit(string msg);
        
    }
}
