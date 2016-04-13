using Models.Communication;

namespace FrontEnd.Core.Event.WaitingRoomEvent
{
    public abstract class AbstractSessionEvent : IEvent
    {
        public bool SendToServer { get; set; } = true;
        public bool Broadcast { get; protected set; }
        public abstract SessionEventMessage.OnlineSessionEventType Type { get; }
    }
}