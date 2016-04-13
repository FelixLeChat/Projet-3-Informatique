using System;
using FrontEnd.Core.Event.WaitingRoomEvent;
using FrontEnd.Game.Config.Common;
using Models.Communication;

namespace FrontEnd.Core.Event
{
    public class MatchMakingGameInvitation : AbstractSessionEvent
    {
        public override SessionEventMessage.OnlineSessionEventType Type { get; } = SessionEventMessage.OnlineSessionEventType.MatchMakingGameInvitation;

        public MatchMakingGameInvitation(Guid syncId, string gameId)
        {
            SyncId = syncId;
            GameId = gameId;
            Broadcast = true;
        }

        public string GameId { get; set; }
        public Guid SyncId { get; set; }
    }
}