using FrontEnd.Game.Wrap;
using FrontEnd.ProfileHelper;
using Models.Communication;

namespace FrontEnd.Core.Event.WaitingRoomEvent
{
    /// <summary>
    /// When a started matched is over (normal game over, host forfeit,...)
    /// </summary>
    public class EndOnlineGameEvent : AbstractSessionEvent
    {
        public override SessionEventMessage.OnlineSessionEventType Type { get; } = SessionEventMessage.OnlineSessionEventType.EndOnlineGameEvent;

        public EndGameType EndCause { get; set; }

        /// <summary>
        /// Show Message to the user explaining the cause of the termination
        /// </summary>
        /// <param name="cause"></param>
        public static void ExplainCauseToUser(EndGameType cause)
        {
            switch (cause)
            {
                case EndGameType.Forfeit:
                    MessageHelper.ShowMessage("Le bal est fini!", "Le reine de la place a quitté rageusement la place!");
                    break;
            }
        }
    }
}