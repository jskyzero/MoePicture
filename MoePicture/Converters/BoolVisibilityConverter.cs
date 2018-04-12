using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace MoePicture.Converters
{
    /// <summary>
    /// 双向值转换器：bool -> Visibility
    /// </summary>
    internal class BoolVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language) =>
            (bool)value ^ (parameter as string ?? string.Empty).Equals("Reverse") ?
                Visibility.Visible : Visibility.Collapsed;

        public object ConvertBack(object value, Type targetType, object parameter, string language) =>
            (Visibility)value == Visibility.Visible ^ (parameter as string ?? string.Empty).Equals("Reverse");
    }
}