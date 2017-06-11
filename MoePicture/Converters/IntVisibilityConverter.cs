using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace MoePicture.Converters
{
    // 单向值转换器：int -> Visibility
    internal class IntVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int count = (int)value;
            return count != 0 ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
