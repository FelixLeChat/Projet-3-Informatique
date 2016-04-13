using System.Web;
using Microsoft.Web.WebSockets;

namespace PrincessAPI.Websocket
{
    public class WsConnexionHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            if (context.IsWebSocketRequest)
                context.AcceptWebSocketRequest(new ConnexionWebsocket());
        }

        public bool IsReusable { get { return false; } }
    }
}