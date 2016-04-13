using System;
using FrontEnd.Core.Event.WaitingRoomEvent;
using FrontEnd.Player;
using Models.Communication;

namespace FrontEnd.Core.Event
{
    public class MatchMakingSearchingEvent : AbstractSessionEvent
    {
        public MatchMakingSearchingEvent(Guid syncId)
        {
            Broadcast = true;
            SyncId = syncId;
            PlayerId = Profile.Instance.CurrentProfile.UserHashId;

        }

        public override SessionEventMessage.OnlineSessionEventType Type { get; } = SessionEventMessage.OnlineSessionEventType.MatchMakingSearchingEvent;

        public Guid SyncId { get; private set; }
        public string PlayerId { get; private set; }
        
    }
}