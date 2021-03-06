﻿using System;
using System.Windows;
using System.Windows.Data;

namespace FrontEnd.UserControl.Converter
{
    public class InverseBoolToVisibilityConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof (Visibility))
                throw new InvalidOperationException("The target must be a boolean");

            if (!(bool) value)
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}