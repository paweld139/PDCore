using System;
using System.Windows.Data;

namespace PDCore.Helpers.WPF.Converters
{
    public class IntAndNotNullToBooleanMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Length == 2 && values[0] is bool valueBool)
            {
                if (valueBool && values[1] == null)
                {
                    return false;
                }
            }

            return true;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
