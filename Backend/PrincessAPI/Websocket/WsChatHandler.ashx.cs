using System.Web;
using Microsoft.Web.WebSockets;

namespace PrincessAPI.Websocket
{
    public class WsChatHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            if (context.IsWebSocketRequest)
                context.AcceptWebSocketRequest(new ChatWebsocket());
        }

        public bool IsReusable { get { return false; } }
    }
}