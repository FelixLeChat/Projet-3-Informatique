using FrontEnd.Game.Wrap;

namespace FrontEnd.Core.Event
{
    /// <summary>
    /// Represent event from the ui (usually click)
    /// </summary>
    public class EndGameEvent : IEvent
    {
        public EndGameEvent(EndGameType type, bool returnMenu = true)
        {
            Info = type;
        }

        public EndGameType Info { get; set; }
        public bool ReturnMenu { get; set; }
    }
}