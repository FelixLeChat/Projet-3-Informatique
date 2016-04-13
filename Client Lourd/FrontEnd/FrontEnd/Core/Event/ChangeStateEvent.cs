namespace FrontEnd.Core.Event
{
    public class ChangeStateEvent : IEvent
    {
        public Enums.States NextState { get; set; }
    }
}