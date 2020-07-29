using System;
using System.Globalization;
using System.Windows.Data;

namespace PDCore.Helpers.WPF.Converters
{
    public class IntToUriConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int source)
            {
                if (source == 120 || source == 60 || source == 90 || source == 210 || source == 180)
                    return new Uri("../Images/File.png", UriKind.Relative);
                else
                    return new Uri("../Images/FolderOpen.png", UriKind.Relative);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // To keep it simple, no need to convert back 
            return null;
        }
    }
}
