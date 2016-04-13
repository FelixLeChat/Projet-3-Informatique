using FrontEnd.ProfileHelper;
using Models.Communication;

namespace FrontEnd.Core.Event.WaitingRoomEvent
{
    /// <summary>
    /// Event when the Session is terminate by other way of a game over
    /// (Note the diffence with EndGameEvent)
    /// </summary>
    public class TerminateSessionEvent : AbstractSessionEvent
    {
        public enum TerminationCause
        {
            PrematureQuit,
            HostDisconnection
        }

        public override SessionEventMessage.OnlineSessionEventType Type { get; } = SessionEventMessage.OnlineSessionEventType.TerminateOnlineSessionEvent;

        public TerminationCause Cause { get; set; }

        /// <summary>
        /// Show Message to the user explaining the cause of the termination
        /// </summary>
        /// <param name="cause"></param>
        public static void ExplainCauseToUser(TerminationCause cause)
        {
            switch (cause)
            {
                case TerminationCause.PrematureQuit:
                    MessageHelper.ShowMessage("Mauvaise nouvelle!", "La reine du bal a oublié de fermer son four, le bal est annulé");
                    break;

                case TerminationCause.HostDisconnection:
                    MessageHelper.ShowMessage("Le bal est terminé", "La reine du bal a quitté pour des raisons inconnus");
                    break;
            }
        }
    }
}