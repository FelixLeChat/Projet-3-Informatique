using System.Web;
using Microsoft.Web.WebSockets;

namespace PrincessAPI.Websocket
{
    public class WsGameHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            if (context.IsWebSocketRequest)
                context.AcceptWebSocketRequest(new GameWebsocket());
        }

        public bool IsReusable { get { return false; } }
    }
}