namespace FrontEnd.Core.Event
{
    /// <summary>
    /// Represent event from the ui (usually click)
    /// </summary>
    public class UiEvent : IEvent
    {
        public enum EventInfo
        {
            LaunchGame,
            OfflineGame,
            QuickGame,
            Campaign,
            OnlineGame,
            HostGame,
            OnlineBoard,
            MatchMaking,
            Back
        }

        public EventInfo Info { get; set; }
    }
}