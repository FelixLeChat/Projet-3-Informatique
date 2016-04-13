using Microsoft.Web.WebSockets;

namespace PrincessAPI.Websocket
{
    public class SessionEventGameSocket : WebSocketHandler
    {
        private static readonly WebSocketCollection Clients = new WebSocketCollection();
        
        public override void OnOpen()
        {
            Clients.Add(this);
        }

        public override void OnMessage(string message)
        {
            Clients.Broadcast(message);
        }

        public override void OnClose()
        {
            Clients.Remove(this);
        }
    }
}