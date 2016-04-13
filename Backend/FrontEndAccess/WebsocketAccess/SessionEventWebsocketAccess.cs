using System;
using System.Collections.Generic;
using Models.Communication;
using Newtonsoft.Json;
using WebSocketSharp;

namespace FrontEndAccess.WebsocketAccess
{
    public class SessionEventWebsocketAccess
    {
        private static WebSocket _ws;

        public event Action<SessionEventMessage> OnSessionEvent;

        private static object _instanceLock = new object();

        private static SessionEventWebsocketAccess _instance;
        public static SessionEventWebsocketAccess Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                    {
                        throw new Exception("OnlineEventWebSocket must be Initialize before been called on");
                    }
                }
                return _instance;
            }
        }

        public static void Initialize(string endpoint)
        {
            lock (_instanceLock)
            {
                if (_instance == null)
                {
                    _instance = new SessionEventWebsocketAccess(endpoint);
                }
            }
        }

        private SessionEventWebsocketAccess(string endpoint)
        {
            // Websocket initialisation
            _ws = new WebSocket(endpoint);
            _ws.OnMessage += WsOnOnMessage;
            _ws.OnClose += WsOnClose;
        }

        private void WsOnOnMessage(object sender, MessageEventArgs messageEventArgs)
        {
            string content = messageEventArgs.Data;

            var decodedError = JsonConvert.DeserializeObject<ErrorMessage>(content);
            if (!string.IsNullOrWhiteSpace(decodedError.Error))
            {
                Console.WriteLine("Error received from the online event websocket: {0}", decodedError.Error);
            }

            var decodedMessage = JsonConvert.DeserializeObject<SessionEventMessage>(content);
            if (!string.IsNullOrWhiteSpace(decodedMessage.JsonEvent))
            {
                var handlers = OnSessionEvent;
                handlers?.Invoke(decodedMessage);
            }
        }

        private void WsOnClose(object sender, CloseEventArgs messageEventArgs)
        {
            Console.WriteLine("Something close the connexion with the websocket");
        }

        /// <summary>
        /// Send a message in the specified chat canal
        /// </summary>
        /// <param name="message"></param>
        /// <param name="chatId"></param>
        /// <param name="userToken"></param>
        public void SendMessage(SessionEventMessage sessionEvent)
        {
            if (!sessionEvent.IsValid())
            {
                return;
            }

            Send(sessionEvent);
        }

        /// <summary>
        /// Send the data to the websocket endpoint
        /// </summary>
        /// <param name="message"></param>
        private void Send(SessionEventMessage message)
        {
            var obj = JsonConvert.SerializeObject(message);
            _ws.Send(obj);
        }

        public void CloseSocket()
        {
            _ws.Close();
        }

        public void OpenSocket()
        {
            _ws.Connect();
        }
    }
}