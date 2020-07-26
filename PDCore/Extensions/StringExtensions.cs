using PDCore.Utils;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;

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
            var orderedCharacters = StringUtils.GetOrderedCharacters(text);

            return string.Concat(orderedCharacters);
        }

        public static bool IsConnectionString(this string text)
        {
            DbConnectionStringBuilder csb = new DbConnectionStringBuilder();

            try
            {
                csb.ConnectionString = text; // throws
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source?.IndexOf(toCheck, comp) >= 0;
        }

        public static bool IsUrl(this string urlOrFilename) => urlOrFilename.ToLower().StartsWith("http");

        public static string ToNumberString(this string value, int precision)
        {
            if (!double.TryParse(value, out double numberValue))
                return value;

            string format = string.Format("{{0:N{0}}}", precision);

            string valuestring = string.Format(format, numberValue);

            return valuestring;
        }

        public static T ToEnumValue<T>(this string enumerationDescription) where T : struct
        {
            var type = typeof(T);

            if (!type.IsEnum)
                throw new ArgumentException("ToEnumValue<T>(): Must be of enum type", "T");

            foreach (T val in EnumUtils.GetEnumValues<T>())
                if (val.GetDescription() == enumerationDescription)
                    return val;

            throw new ArgumentException("ToEnumValue<T>(): Invalid description for enum " + type.Name, "enumerationDescription");
        }

        public static bool IsPalindrome(this string input)
        {
            var forwards = input.Replace(" ", "");

            var backwards = new string(forwards.Reverse().ToArray());

            return backwards.Equals(forwards);
        }

        public static char LastCharacter(this string input)
        {
            int lastIndex = input.LastIndex();

            return input[lastIndex];
        }

        public static int LastIndex(this string input) => input.Length - 1;

        public static string RemoveLastCharacter(this string input)
        {
            int lastIndex = input.LastIndex();

            return input.Remove(lastIndex);
        }

        public static string[] GetSentences(this string input)
        {
            return Regex.Split(input, @"(?<=[\.!\?])\s+");
        }

        public static string TrimSuffix(this string word)
        {
            int apostropheLocation = word.IndexOf('\'');

            if (apostropheLocation != -1)
            {
                word = word.Substring(0, apostropheLocation);
            }

            return word;
        }

        public static string[] GetWords(this string input)
        {
            MatchCollection matches = Regex.Matches(input, @"\b[\w'-]*\b");

            var words = from m in matches.Cast<Match>()
                        where !string.IsNullOrEmpty(m.Value)
                        select TrimSuffix(m.Value);

            return words.ToArray();
        }
    }
}
