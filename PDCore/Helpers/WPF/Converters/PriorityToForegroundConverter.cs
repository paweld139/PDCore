using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace PDCore.Helpers.WPF.Converters
{
    public class PriorityToForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Brush))
            {
                throw new InvalidOperationException("Celem powinien być typ Brush");
            }

            int priorytet = System.Convert.ToInt32(value);

            return (priorytet == 1 ? Brushes.Red : Brushes.Black);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
