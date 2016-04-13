using Models.Database;

namespace FrontEnd.Core.Event
{
    public class ReconnectToGameEvent : IEvent
    {
        public GameModel Game { get; set; } 
    }
}