using System.Web;
using Microsoft.Web.WebSockets;

namespace PrincessAPI.Websocket
{
    public class WsSessionEventHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            if (context.IsWebSocketRequest)
                context.AcceptWebSocketRequest(new SessionEventGameSocket());
        }

        public bool IsReusable { get { return false; } }
    }
}