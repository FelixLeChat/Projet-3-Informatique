using FrontEnd.Game.Config.Common;

namespace FrontEnd.Core.Event
{
    public class CreateGameRequestEvent : IEvent
    {
         public IGameConfig Config { get; set; }
    }
}