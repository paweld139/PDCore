using PDCore.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Text;

namespace PDCore.Extensions
{
    public static class StringExtensions
    {
        public static string ToTitleCase(this string text)
        {
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;

            return textInfo.ToTitleCase(text);
        }

        public static SecureString GetSecureString(this string text)
        {
            SecureString secureString = new SecureString();

            text.ToCharArray().ForEach(p => secureString.AppendChar(p));


            return secureString;
        }

        public static StringBuilder AppendLine(this StringBuilder builder, string format, params object[] args)
        {
            builder.AppendFormat(format, args).AppendLine();

            return builder;
        }

        /// <summary>
        /// Konwersja stringa do enuma
        /// </summary>
        /// <typeparam name="TEnum">Typ enuma</typeparam>
        /// <param name="value"></param>
        /// <returns>Enum</returns>
        public static TEnum ParseEnum<TEnum>(this string value) where TEnum : struct
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value);
        }

        public static string[] Split(this string text, StringSplitOptions stringSplitOptions, params string[] delimiters)
        {
            return text.Split(delimiters, stringSplitOptions);
        }

        public static string[] Split(this string text, params string[] delimiters)
        {
            return text.Split(delimiters, StringSplitOptions.None);
        }

        /// <summary>
        /// Dodanie spacji przed każdą częścią łańcucha znaków zaczynającą się od dużej litery
        /// </summary>
        /// <param name="text">Łańcuch znaków do przetworzenia</param>
        /// <returns>Przetworzony łańcuch znaków</returns>
        public static string AddSpaces(this string text)
        {
            string result = string.Empty; //Jest ustawiony pusty łańcuch znaków, żeby zwrócona wartość nie była nullem, w przypadku gdy parametr jest pusty lub jest nullem

            if (!string.IsNullOrWhiteSpace(text)) //Przekazany łańcuch znaków nie może być pusty, ani posiadać wartość null (pustą referencję)
            {
                foreach (char item in text) //Przechodzimy po każdym znaku łańcucha znaków
                {
                    if (char.IsUpper(item)) //Znak jest dużą literą
                    {
                        result = result.Trim(); //Zostają usunięte wszystkie białe znaki znajdujące się na początku i na końcu łańcucha znaków do zwrócenia

                        result += " "; //Dodanie spacji na końcu stringa
                    }

                    result += item; //Dodanie litery do stringa
                }

                result = result.Trim();
            }

            return result; //Zwrócenie przetworzonego stringa
        }

        /// <summary>
        /// Dodanie białych znaków po lewej i prawej stronie łańcucha znaków, gdy jego ilość znaków jest mniejsza od przekazanej
        /// </summary>
        /// <param name="text">Łańcuch znaków</param>
        /// <param name="totalWidth">Całkowita szerokość, ilość znaków</param>
        /// <returns></returns>
        public static string PadBoth(this string text, int totalWidth)
        {
            int spaces = totalWidth - text.Length; //Ilość białych znaków, czyli całkowita długość minus długość tekstu
            int padLeft = spaces / 2 + text.Length;
            //Ilość znaków jaką musi zajmować tekst, jeżeli tak nie jest, to zostają dodane z lewej strony. Ilość tych znaków to połowa ilość białych znaków powiększona o długość tekstu

            return text.PadLeft(padLeft).PadRight(totalWidth);
            //Przesunięcie w lewo i w prawu tekstu, by był na najbardziej na środku, najpierw ilość znaków to połowa ilość znaków powiększona o długość tekstu, a później z prawej strony całkowita ilość znaków
        }

        public static double ToDouble(this string data)
        {
            double result = double.Parse(data);

            return result;
        }

        public static string Order(this string text)
        {
            var orderedCharacters = ObjectUtils.GetOrderedCharacters(text);

            return string.Concat(orderedCharacters);
        }
    }
}
