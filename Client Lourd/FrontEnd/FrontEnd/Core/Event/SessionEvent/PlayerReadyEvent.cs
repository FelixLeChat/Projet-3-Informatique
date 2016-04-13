using Models.Communication;

namespace FrontEnd.Core.Event.WaitingRoomEvent
{
    public class PlayerReadyEvent : AbstractSessionEvent
    {
        public string PlayerId { get; set; }
        public override SessionEventMessage.OnlineSessionEventType Type { get; } = SessionEventMessage.OnlineSessionEventType.PlayerReadyEvent;
    }
}