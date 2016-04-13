using System;
using System.Windows.Data;

namespace FrontEnd.UserControl.Converter
{
    public class InvitationStateToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var invitationSent = (bool) value;
            return invitationSent ? "Invitation envoyé" : "Inviter";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}