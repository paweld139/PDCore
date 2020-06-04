using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace PDCore.Helpers.WPF.DataValidation
{
    public class NumberValidator : ValidationRule
    {
        public double Min { get; set; }
        public double Max { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            double sprawdzanaLiczba = 0;

            try
            {
                string valueString = value.ToString();

                if (valueString.Length > 0)
                    sprawdzanaLiczba = double.Parse(valueString);
            }
            catch (FormatException e)
            {
                return new ValidationResult(false, "Niedozwolone znaki - " + e.Message);
            }

            if (sprawdzanaLiczba < Min || sprawdzanaLiczba > Max)
            {
                return new ValidationResult(false, "Wprowadź liczbę z zakresu: " + Min + " - " + Max);
            }
            else
            {
                return new ValidationResult(true, null);
            }
        }
    }
}
