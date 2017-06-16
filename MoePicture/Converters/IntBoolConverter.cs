using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace MoePicture.Converters
{
    public class IntBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int count = (int)value;
            return count == 0 ? true : false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
