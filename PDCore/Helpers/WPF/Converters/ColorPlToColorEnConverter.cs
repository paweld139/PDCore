using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Data;

namespace PDCore.Helpers.WPF.Converters
{
    public class ColorPlToColorEnConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Brush))
            {
                throw new InvalidOperationException("Celem powinien być typ Brush");
            }

            string kolorPL = value.ToString(); //Kolor w języku polskim jako string

            Dictionary<string, Brush> kolory = new Dictionary<string, Brush>
            {
                { "Czarny", Brushes.Black },
                { "Czerwony", Brushes.Red },
                { "Żółty", Brushes.Yellow },
                { "Zielony", Brushes.Green },
                { "Niebieski", Brushes.Blue }
            };


            kolory.TryGetValue(kolorPL, out Brush result);

            return result ?? Brushes.LightGray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
