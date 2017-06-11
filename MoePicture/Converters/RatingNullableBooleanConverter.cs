using System;
using Windows.UI.Xaml.Data;

namespace MoePicture.Converters
{
    // 双向值转换器：UserConfigItem.rating(枚举) -> bool?
    internal class RatingNullableBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var rating = (Models.UserConfig.rating)value;
            return rating == Models.UserConfig.rating.all ? (bool?)true : (bool?)false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var LoadAll = (bool?)value;
            return LoadAll == true ? Models.UserConfig.rating.all : Models.UserConfig.rating.safe;
        }
    }
}
