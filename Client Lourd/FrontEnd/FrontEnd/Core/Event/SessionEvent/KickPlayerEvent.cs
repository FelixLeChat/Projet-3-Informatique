using Models.Communication;

namespace FrontEnd.Core.Event.WaitingRoomEvent
{
    public class KickPlayerEvent : AbstractSessionEvent
    {
        public string KickedPlayerId { get; set; }
        public override SessionEventMessage.OnlineSessionEventType Type { get; } = SessionEventMessage.OnlineSessionEventType.KickPlayerEvent;
    }
}