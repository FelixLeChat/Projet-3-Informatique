using System;
using System.Windows;
using System.Windows.Data;
using FrontEnd.ProfileHelper;
using Helper.Image;
using Models;

namespace FrontEnd.UserControl.Converter
{
    public class WantedEnumToVisibilityConverter : IValueConverter
    {
        // If the enum is the same as the one wanted, it returned visible, hidden otherwise
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.Equals(parameter) ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.Equals(true) ? parameter : Binding.DoNothing;
        }
    }
}