using System.Windows;
using FrontEnd.Game;

namespace FrontEnd.UserControl.CustomRoutedEvent
{
    public class ZoneConfigChange : RoutedEventArgs
    {
        private readonly ZoneConfig _zoneConfig;

        public ZoneConfig ZoneConfig => _zoneConfig;

        public ZoneConfigChange(RoutedEvent routedEvent, ZoneConfig zoneConfig) : base(routedEvent) {
            this._zoneConfig = zoneConfig;
        }
    }
}