﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace PDCore.Extensions
{
    /// <summary>
    /// Statyczna klasa rozszerzająca przydatna przy operacjach na datach
    /// </summary>
    public static class DateTimeExtension
    {
        /// <summary>
        /// Zwrócenie łańcucha znaków o formacie "rok-miesiąc-dzień", opcjonalnie także z czasem dla zadanej daty
        /// </summary>
        /// <param name="dt">Data, dla której zostanie zwrócony łańcuch znaków</param>
        /// <param name="withHours">Określenie czy zwracany łańcuch znaków ma zawierać czas</param>
        /// <returns>Łańcuch znaków będący odpowiednikiem przekazanej daty</returns>
        public static string ToYMD(this DateTime dt, bool withHours = true)
        {
            return dt.ToString("yyyy-MM-dd" + (withHours ? " HH:mm:ss" : string.Empty)); //Konwersja daty do odpowiedniego formatu i zwrócenie łańcucha znaków
        }

        /// <summary>
        /// Zwrócenie łańcucha znaków o formacie "rok-miesiąc-dzień", opcjonalnie także z czasem dla zadanej daty w formie łańcucha znaków
        /// </summary>
        /// <param name="s">Data w formie łańcucha znaków, dla której zostanie zwrócony łańcuch znaków</param>
        /// <param name="withHours">Określenie czy zwracany łańcuch znaków ma zawierać czas</param>
        /// <returns>Łańcuch znaków będący odpowiednikiem przekazanej daty w formie łańcucha znaków</returns>
        public static string ToYMD(this string s, bool withHours = true)
        {
            DateTime dt = Convert.ToDateTime(s); //Konwersja łańcucha znaków na datę

            return dt.ToString("yyyy-MM-dd" + (withHours ? " HH:mm:ss" : string.Empty)); //Konwersja daty do odpowiedniego formatu i zwrócenie łańcucha znaków
        }

        /// <summary>
        /// Zwrócenie łańcucha znaków o formacie "dzień-miesiąc-rok", opcjonalnie także z czasem dla zadanej daty
        /// </summary>
        /// <param name="dt">Data, dla której zostanie zwrócony łańcuch znaków</param>
        /// <param name="withHours">Określenie czy zwracany łańcuch znaków ma zawierać czas</param>
        /// <returns>Łańcuch znaków będący odpowiednikiem przekazanej daty</returns>
        public static string ToDMY(this DateTime dt, bool withHours = true)
        {
            return dt.ToString("dd-MM-yyyy" + (withHours ? " HH:mm:ss" : string.Empty)); //Konwersja daty do odpowiedniego formatu i zwrócenie łańcucha znaków
        }

        /// <summary>
        /// Zwrócenie liczby (całkowita podwójnej precyzji - 64 bitowa) o formacie "rok-miesiąc-dzień", opcjonalnie także z czasem dla zadanej daty
        /// </summary>
        /// <param name="dateTime">Data, dla której zostanie zwrócona liczba</param>
        /// <param name="withHours">Określenie czy zwracana liczba ma zawierać czas</param>
        /// <returns></returns>
        public static long GetLong(this DateTime dateTime, bool withHours = true)
        {
            return long.Parse(dateTime.ToString("yyyyMMdd" + (withHours ? "HHmmss" : string.Empty)));
        }

        public static string GetWordly(this DateTime dateTime)
        {
            CultureInfo cultureInfo = new CultureInfo("pl-PL");

            return dateTime.ToString("d MMMM yyyy r.", cultureInfo);
        }

        public static string GetTime(this DateTime dt, bool withSeconds = true)
        {
            return dt.ToString("HH:mm" + (withSeconds ? ":ss" : string.Empty));
        }

        public static int DaysToEndOfMonth(this DateTime date)
        {
            return DateTime.DaysInMonth(date.Year, date.Month) - date.Day;
        }
    }
}
