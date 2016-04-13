
namespace FrontEnd.Core.Event
{
    public class HostJoiningCreatedGameEvent : IEvent
    {
        public string HashId { get; set; }
    }
}