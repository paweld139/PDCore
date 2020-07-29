using System;
using System.Globalization;
using System.Windows.Data;

namespace PDCore.Helpers.WPF.Converters
{
    public class EnumToBooleanConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;

            string checkValue = value.ToString(); //string z enuma

            string targetValue = parameter.ToString(); //wartość enuma, która oznacza true

            return checkValue.Equals(targetValue, StringComparison.InvariantCultureIgnoreCase);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return null;

            bool useValue = (bool)value;

            string targetValue = parameter.ToString(); //wartość enuma, która oznacza true

            if (useValue)
                return Enum.Parse(targetType, targetValue);

            return null;
        }
    }

}
