using System.Windows;

namespace FrontEnd.UserControl.SpecializedUserControl
{
    /// <summary>
    /// User Control that can bubble up events to the parents
    /// How to use: In the xaml of the child: inherits from ChildControlBase instead of UserControl
    /// (source: https://www.stevefenton.co.uk/2012/09/WPF-Bubbling-A-Command-From-A-Child-View/)
    /// </summary>
    public class ChildControlBase : System.Windows.Controls.UserControl
    {
        // This defines the custom event
        public static readonly System.Windows.RoutedEvent MyCustomEvent = EventManager.RegisterRoutedEvent(
            "ChildEvent", // Event name
            RoutingStrategy.Bubble, // Bubble means the event will bubble up through the tree
            typeof(RoutedEventHandler), // The event type
            typeof(ChildControlBase)); // Belongs to ChildControlBase

        // Allows add and remove of event handlers to handle the custom event
        public event RoutedEventHandler ChildEvent
        {
            add { AddHandler(MyCustomEvent, value); }
            remove { RemoveHandler(MyCustomEvent, value); }
        }
    }
}