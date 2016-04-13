using System;
using System.Windows.Data;
using System.Windows.Media;
using FrontEnd.Game;
using FrontEnd.Model.ViewModel;

namespace FrontEnd.UserControl.Converter
{
    public class IsReadyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var state = value as FriendInvitationItem.State? ?? FriendInvitationItem.State.Offline;
            switch (state)
            {
                case FriendInvitationItem.State.Online:
                    return new SolidColorBrush(Colors.Aquamarine);
                default:
                    return new SolidColorBrush(Colors.Gray);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}