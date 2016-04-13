using System;
using System.Diagnostics;
using System.Windows;
using FrontEnd.Core.Event;
using FrontEnd.Core.Event.WaitingRoomEvent;
using FrontEnd.Game;
using FrontEnd.Player;
using FrontEndAccess.WebsocketAccess;
using Models.Communication;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using EventManager = FrontEnd.Core.EventManager;

namespace FrontEnd.WebCommunication
{
    /// <summary>
    /// Listen to the session websocket and the event from the game, send event to each other when it is session event
    /// </summary>
    public class OnlineEventSubscriber : IDisposable
    {
        private static readonly object _instanceLock = new object();

        private static OnlineEventSubscriber _instance;
        public static OnlineEventSubscriber Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = new OnlineEventSubscriber();
                    }
                }
                return _instance;
            }
        }

        private OnlineEventSubscriber()
        {
            EventManager.Instance.Subscribe(SessionEventReceivedFromLocal);

            SessionEventWebsocketAccess.Instance.OnSessionEvent += OnlineEventFromServerSubscriber;
        }

        public void Dispose()
        {
            SessionEventWebsocketAccess.Instance.OnSessionEvent -= OnlineEventFromServerSubscriber;
        }

        public static void Initiate()
        {
            // Todo: beurk!
            var onlineEventSubscriber = Instance;
        }

        /// <summary>
        /// Listen the online event, convert it to the right format and send it to the local event manager
        /// </summary>
        public void OnlineEventFromServerSubscriber(SessionEventMessage sessionEvent)
        {
            if (sessionEvent.Broadcast || (OnlineSession.Instance != null && sessionEvent.OnlineSessionId == OnlineSession.Instance.SessionId)) 
            {
                var converted = Convert(sessionEvent);
                Debug.Assert(converted != null, "SessionEvent must can be converted in IEvent");
                Application.Current.Dispatcher.Invoke(() => { EventManager.Instance.Notice(converted); });
            }
        }

        /// <summary>
        /// Listen to local event and send it to the server if needed
        /// </summary>
        public void SessionEventReceivedFromLocal(IEvent localEvent)
        {
            var sessionEvent = localEvent as AbstractSessionEvent;
            if (sessionEvent != null && sessionEvent.SendToServer)
            {
                //Debug.Assert(OnlineSession.Instance != null, "Should not send Session event after exiting it");
                var converted = Convert(sessionEvent);
                Debug.Assert(converted != null, "AbstractSessionEvent must can be converted in SessionEvent");
                SessionEventWebsocketAccess.Instance.SendMessage(converted);
            }
        }

        private AbstractSessionEvent Convert(SessionEventMessage message)
        {

            AbstractSessionEvent sessionEvent = null;
            switch ((SessionEventMessage.OnlineSessionEventType)message.EventType)
            {
                case SessionEventMessage.OnlineSessionEventType.KickPlayerEvent:
                    sessionEvent = JsonConvert.DeserializeObject<KickPlayerEvent>(message.JsonEvent);
                    break;
                case SessionEventMessage.OnlineSessionEventType.TerminateOnlineSessionEvent:
                    sessionEvent = JsonConvert.DeserializeObject<TerminateSessionEvent>(message.JsonEvent);
                    break;
                case SessionEventMessage.OnlineSessionEventType.StartOnlineGameEvent:
                    sessionEvent = JsonConvert.DeserializeObject<StartGameEvent>(message.JsonEvent);
                    break;
                case SessionEventMessage.OnlineSessionEventType.PlayerReadyEvent:
                    sessionEvent = JsonConvert.DeserializeObject<PlayerReadyEvent>(message.JsonEvent);
                    break;
                case SessionEventMessage.OnlineSessionEventType.EndOnlineGameEvent:
                    sessionEvent = JsonConvert.DeserializeObject<EndOnlineGameEvent>(message.JsonEvent);
                    break;
                case SessionEventMessage.OnlineSessionEventType.PlayerForfeitEvent:
                    sessionEvent = JsonConvert.DeserializeObject<PlayerForfeitEvent>(message.JsonEvent);
                    break;
                case SessionEventMessage.OnlineSessionEventType.InvitationEvent:
                    sessionEvent = JsonConvert.DeserializeObject<InvitationEvent>(message.JsonEvent);
                    break;
                case SessionEventMessage.OnlineSessionEventType.MatchMakingSearchingEvent:
                    sessionEvent = JsonConvert.DeserializeObject<MatchMakingSearchingEvent>(message.JsonEvent);
                    break;
                case SessionEventMessage.OnlineSessionEventType.MatchMakingSyncEvent:
                    sessionEvent = JsonConvert.DeserializeObject<MatchMakingSyncEvent>(message.JsonEvent);
                    break;
                case SessionEventMessage.OnlineSessionEventType.MatchMakingGameInvitation:
                    sessionEvent = JsonConvert.DeserializeObject<MatchMakingGameInvitation>(message.JsonEvent);
                    break;
            }
            if (sessionEvent != null)
                sessionEvent.SendToServer = false;

            return sessionEvent;
        }

        private SessionEventMessage Convert(AbstractSessionEvent sessionEvent)
        {
            var message = new SessionEventMessage()
            {
                EventType = (int)sessionEvent.Type,
                JsonEvent = JsonConvert.SerializeObject(sessionEvent),
                OnlineSessionId = OnlineSession.Instance?.SessionId ?? "",
                UserToken = User.Instance.UserToken,
                Broadcast = sessionEvent.Broadcast
            };

            return message;
        }
    }
}