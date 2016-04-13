using System;
using FrontEnd.Core.Event.WaitingRoomEvent;
using FrontEnd.Game.Config.Common;
using FrontEnd.Player;
using Models.Communication;

namespace FrontEnd.Core.Event
{
    public class MatchMakingSyncEvent : AbstractSessionEvent
    {
        public override SessionEventMessage.OnlineSessionEventType Type { get; } = SessionEventMessage.OnlineSessionEventType.MatchMakingSyncEvent;

        public enum SyncStep
        {
            HostWantTargetPlayer,
            PlayerWillWaitForHostInvitation
        }


        public MatchMakingSyncEvent(Guid syncId, SyncStep currentSyncStep)
        {
            SyncId = syncId;
            CurrentSyncStep = currentSyncStep;
            Broadcast = true;
        }

        public SyncStep CurrentSyncStep { get; set; }
        public Guid SyncId { get; set; }
    }
}