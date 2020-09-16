using PDCore.Interfaces;
using System;
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

            return dt.ToYMD(withHours); //Konwersja daty do odpowiedniego formatu i zwrócenie łańcucha znaków
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

        public static string ToDMY(this string s, bool withHours = true)
        {
            DateTime dt = Convert.ToDateTime(s); //Konwersja łańcucha znaków na datę

            return dt.ToDMY(withHours); //Konwersja daty do odpowiedniego formatu i zwrócenie łańcucha znaków
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

        public static string GetWordly(this DateTime dateTime, CultureInfo cultureInfo)
        {
            return dateTime.ToString("d MMMM yyyy r.", cultureInfo);
        }

        public static string GetWordly(this DateTime dateTime, string cultureInfoName)
        {
            CultureInfo cultureInfo = new CultureInfo(cultureInfoName);

            return dateTime.GetWordly(cultureInfo);
        }

        public static string GetWordlyPL(this DateTime dateTime)
        {
            return dateTime.GetWordly("pl-PL");
        }

        public static string GetWordlyGB(this DateTime dateTime)
        {
            return dateTime.GetWordly("en-GB");
        }

        public static string GetWordlyUS(this DateTime dateTime)
        {
            return dateTime.GetWordly("en-US");
        }

        public static string GetWordlyDE(this DateTime dateTime)
        {
            return dateTime.GetWordly("de-DE");
        }

        public static string GetWordlyFR(this DateTime dateTime)
        {
            return dateTime.GetWordly("fr-FR");
        }

        public static string GetWordlyJA(this DateTime dateTime)
        {
            return dateTime.GetWordly("ja-JA");
        }

        public static string GetTime(this DateTime dt, bool withSeconds = true)
        {
            return dt.ToString("HH:mm" + (withSeconds ? ":ss" : string.Empty));
        }

        public static int DaysToEndOfMonth(this DateTime date)
        {
            return DateTime.DaysInMonth(date.Year, date.Month) - date.Day;
        }

        public static string ToISO8601(this DateTime dateTime) => dateTime.ToString("o", CultureInfo.InvariantCulture);

        public static bool IsBetween(this DateTime source, DateTime start, DateTime end) => source > start && source < end;

        public static DateTimeOffset GetEndDate(this IPeriod period) => period.StartDate.Add(period.Duration);

        public static TimeSpan GetTimeSince(this IPeriod period) => DateTimeOffset.UtcNow.ToOffset(period.StartDate.Offset) - period.StartDate;

        /// <summary>
        /// Returns TimeZone adjusted time for a given from a Utc or local time.
        /// Date is first converted to UTC then adjusted.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timeZoneId"></param>
        /// <returns></returns>
        public static DateTime ToTimeZoneTime(this DateTime time, string timeZoneId = "Pacific Standard Time")
        {
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return time.ToTimeZoneTime(tzi);
        }

        /// <summary>
        /// Returns TimeZone adjusted time for a given from a Utc or local time.
        /// Date is first converted to UTC then adjusted.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timeZoneId"></param>
        /// <returns></returns>
        public static DateTime ToTimeZoneTime(this DateTime time, TimeZoneInfo tzi)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(time, tzi);
        }
    }
}
