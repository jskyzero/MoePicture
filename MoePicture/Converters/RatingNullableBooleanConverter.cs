using System;
using Windows.UI.Xaml.Data;

namespace MoePicture.Converters
{
    // 双向值转换器：UserConfigItem.rating(枚举) -> bool?
    internal class RatingNullableBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var rating = (Models.RatingType)value;
            return rating == Models.RatingType.All ? (bool?)true : (bool?)false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var LoadAll = (bool?)value;
            return LoadAll == true ? Models.RatingType.All : Models.RatingType.Safe;
        }
    }
}
