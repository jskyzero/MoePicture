using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace MoePicture.Converters
{
    public class TagStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string siteName = ServiceLocator.Current.GetInstance<ViewModels.PictureItemsVM>().Type.ToString();
            siteName = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(siteName.ToLower());
            return (string)value == String.Empty ? siteName + " Home" : ("#" + (string)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}