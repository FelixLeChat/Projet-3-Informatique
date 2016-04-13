using Models.Communication;

namespace FrontEnd.Core.Event.WaitingRoomEvent
{
    public class PlayerForfeitEvent : AbstractSessionEvent
    {
        public override SessionEventMessage.OnlineSessionEventType Type { get; } = SessionEventMessage.OnlineSessionEventType.PlayerForfeitEvent;
        public string PlayerId { get; set; }

        public PlayerForfeitEvent(string hashId)
        {
            PlayerId = hashId;
        }

        
    }
}