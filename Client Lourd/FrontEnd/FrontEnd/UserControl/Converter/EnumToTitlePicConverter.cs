using System;
using System.Windows.Data;
using FrontEnd.ProfileHelper;
using Helper.Image;
using Models;

namespace FrontEnd.UserControl.Converter
{
    public class EnumToTitlePicConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var convertedValue = value as EnumsModel.PrincessTitle? ?? EnumsModel.PrincessTitle.Lady;

            return ImageHelper.GetImageBrush(EnumToImage.GetRankBitmap(convertedValue));
        }

        // Not sure of this one
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}