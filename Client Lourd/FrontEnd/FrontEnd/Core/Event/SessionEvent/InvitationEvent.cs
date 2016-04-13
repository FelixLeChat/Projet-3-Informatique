using FrontEnd.Core.Event.WaitingRoomEvent;
using FrontEnd.Game.Config.Common;
using Models.Communication;

namespace FrontEnd.Core.Event
{
    public class InvitationEvent : AbstractSessionEvent
    {
        public InvitationEvent()
        {
            Broadcast = true;
        }

        public override SessionEventMessage.OnlineSessionEventType Type { get; } = SessionEventMessage.OnlineSessionEventType.InvitationEvent;

        public string HashId { get; set; }
        public string Password { get; set; }
        public string InvitedPlayerId { get; set; }
        public string InviterId { get; set; }
    }
}