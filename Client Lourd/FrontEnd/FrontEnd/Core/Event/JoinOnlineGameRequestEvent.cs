
namespace FrontEnd.Core.Event
{
    public class JoinOnlineGameRequestEvent : IEvent
    {
        public string HashId { get; set; }
        public string Name { get; set; }
        public bool IsPrivate { get; set; }
        public string Password { get; set; }
    }
}