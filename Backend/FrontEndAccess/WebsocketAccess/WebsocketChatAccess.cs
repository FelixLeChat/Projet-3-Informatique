using System;
using System.Collections.Generic;
using Models.Communication;
using Newtonsoft.Json;
using WebSocketSharp;

namespace FrontEndAccess.WebsocketAccess
{
    public class WebsocketChatAccess
    {
        private static WebSocket _ws;
        private static readonly List<EventHandler<MessageEventArgs>> DelegatesEventHandlers = new List<EventHandler<MessageEventArgs>>();

        public static WebsocketChatAccess Instance { get; set; }

        public WebsocketChatAccess(string endpoint)
        {
            // Websocket initialisation
            _ws = new WebSocket(endpoint);
            Instance = this;
            //OpenSocket();
        }

        /// <summary>
        /// OnMessage can only contain one delegate event
        /// </summary>
        /// <param name="del"></param>
        public void AddEvent(EventHandler<MessageEventArgs> del)
        {
            foreach (var d in DelegatesEventHandlers)
            {
                _ws.OnMessage -= d;
            }
            DelegatesEventHandlers.Add(del);
            _ws.OnMessage += del;
        }

        /// <summary>
        /// Send a message in the specified chat canal
        /// </summary>
        /// <param name="message"></param>
        /// <param name="chatId"></param>
        /// <param name="userToken"></param>
        public void SendMessage(string message, string chatId, string userToken)
        {
            if (string.IsNullOrWhiteSpace(message) || 
                string.IsNullOrWhiteSpace(chatId) ||
                string.IsNullOrWhiteSpace(userToken))
                return;

            var body = new WebsocketChatMessage()
            {
                CanalId = chatId,
                Message = message,
                UserToken = userToken
            };
            Send(body);
        }

        /// <summary>
        /// Create a new canal and receive the canal hash
        /// </summary>
        /// <param name="chatName"></param>
        /// <param name="userToken"></param>
        public void CreateNewCanal(string chatName, string userToken)
        {
            if (string.IsNullOrWhiteSpace(chatName) ||
                string.IsNullOrWhiteSpace(userToken))
                return;
            var body = new WebsocketChatMessage()
            {
                CanalName = chatName,
                UserToken = userToken
            };
            Send(body);
        }

        /// <summary>
        /// Invite a friend to chat with him
        /// </summary>
        /// <param name="friendHashId"></param>
        /// <param name="friendName"></param>
        /// <param name="userToken"></param>
        public void StartChatWithFriend(string friendHashId, string friendName, string userToken)
        {
            if (string.IsNullOrWhiteSpace(friendHashId) ||
                string.IsNullOrWhiteSpace(friendName) ||
                string.IsNullOrWhiteSpace(userToken))
                return;
            var body = new WebsocketChatMessage()
            {
                UserToken = userToken,
                TargetUserName = friendName,
                TargetUserHashId = friendHashId
            };
            Send(body);
        }

        /// <summary>
        /// Send the data to the websocket endpoint
        /// </summary>
        /// <param name="message"></param>
        private void Send(WebsocketChatMessage message)
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
