using System;
using System.Windows.Data;
using FrontEnd.ProfileHelper;
using Helper.Image;
using Models;

namespace FrontEnd.UserControl.Converter
{
    public class EnumToProfilePicConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var convertedValue = value as EnumsModel.PrincessAvatar? ?? EnumsModel.PrincessAvatar.Ariel;

            return ImageHelper.GetImageBrush(EnumToImage.GetAvatarBitmap(convertedValue));
        }

        // Not sure of this one
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}