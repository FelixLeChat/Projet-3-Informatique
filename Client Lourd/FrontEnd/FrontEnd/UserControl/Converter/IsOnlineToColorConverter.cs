using System;
using System.Windows.Data;
using System.Windows.Media;
using FrontEnd.Game;

namespace FrontEnd.UserControl.Converter
{
    public class IsOnlineToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var state = value as SessionParticipant.ParticipantState? ?? SessionParticipant.ParticipantState.Setuping;
            switch (state)
            {
                case SessionParticipant.ParticipantState.ReadyToStart:
                    return new SolidColorBrush(Colors.Aquamarine);
                default:
                    return new SolidColorBrush(Colors.DarkRed);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}