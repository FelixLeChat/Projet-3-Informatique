using WebSocketSharp;

namespace FrontEndAccess.WebsocketAccess
{
    public class ConnexionWebsocketAccess
    {
        private static WebSocket _ws;
        public static ConnexionWebsocketAccess Instance { get; set; }

        public ConnexionWebsocketAccess(string endpoint)
        {
            // Websocket initialisation
            _ws = new WebSocket(endpoint);
            Instance = this;
        }

        private void SendMessage(string userToken)
        {
            if (string.IsNullOrWhiteSpace(userToken))
                return;
            _ws.Send(userToken);
        }

        public void OpenSocket(string userToken)
        {
            _ws.Connect();
            SendMessage(userToken);
        }

        public void CloseSocket()
        {
            _ws.Close();
        }
    }
}
