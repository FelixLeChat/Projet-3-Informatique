using Models.Communication;

namespace FrontEnd.Core.Event.WaitingRoomEvent
{
    public class StartGameEvent: AbstractSessionEvent
    {
        public override SessionEventMessage.OnlineSessionEventType Type { get; } = SessionEventMessage.OnlineSessionEventType.StartOnlineGameEvent;
    }
}