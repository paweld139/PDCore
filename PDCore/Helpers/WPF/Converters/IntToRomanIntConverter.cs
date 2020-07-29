using System;
using System.Globalization;
using System.Windows.Data;

namespace PDCore.Helpers.WPF.Converters
{
    public class IntToRomanIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int source)
            {
                switch (source)
                {
                    case 0: return "0";
                    case 1: return "I";
                    case 2: return "II";
                    case 3: return "III";
                }
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string source = value.ToString();

            switch (source)
            {
                case "0": return 0;
                case "I": return 1;
                case "II": return 2;
                case "III": return 3;
            }

            return source;
        }
    }
}
