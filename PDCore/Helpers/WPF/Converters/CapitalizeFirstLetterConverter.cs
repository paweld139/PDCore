using System;
using System.Globalization;
using System.Windows.Data;

namespace PDCore.Helpers.WPF.Converters
{
    public class CapitalizeFirstLetterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string castValue)
            {
                return char.ToUpper(castValue[0]) + castValue.Substring(1);
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
