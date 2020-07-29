using System;
using System.Windows.Data;
using System.Windows.Input;

namespace PDCore.Helpers.WPF.Converters
{
    public class BoolToCursorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool val = (bool)value;

            if (val)
            {
                return Cursors.Wait;
            }
            else
            {
                return Cursors.Arrow;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
