using System;
using System.Windows;
using System.Windows.Data;

namespace PDCore.Helpers.WPF.Converters
{
    /// <summary>
    /// Jeśli jakiekolwiek false, to Collapsed. Możliwy parametr rev i wtedy jakikolwiek false to Visible.
    /// </summary>
    public class BooleanToVisibilityMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool result = true;

            if (values != null && values.Length > 0)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    if (values[i] is bool valueBool && !valueBool)
                    {
                        result = false;
                    }
                    else
                    {
                        result = false;
                    }
                }
            }
            else
            {
                return false;
            }

            if (parameter != null && parameter.ToString() == "rev")
            {
                return !result ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                return result ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
