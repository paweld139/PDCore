using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
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

            Dictionary<string, Brush> kolory = new Dictionary<string, Brush>();
            kolory.Add("Czarny", Brushes.Black);
            kolory.Add("Czerwony", Brushes.Red);
            kolory.Add("Żółty", Brushes.Yellow);
            kolory.Add("Zielony", Brushes.Green);
            kolory.Add("Niebieski", Brushes.Blue);

            Brush result = Brushes.LightGray;

            kolory.TryGetValue(kolorPL, out result);

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
