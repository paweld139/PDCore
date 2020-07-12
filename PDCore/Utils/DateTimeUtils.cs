using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace PDCore.Utils
{
    public static class DateTimeUtils
    {
        public enum DniTygodnia
        {
            Niedziela = 0,
            Poniedziałek,
            Wtorek,
            Środa,
            Czwartek,
            Piątek,
            Sobota
        }

        public static int GetDayOfWeek(DateTime dt)
        {
            return (int)dt.DayOfWeek;
        }

        public static string GetDayOfWeekName(DateTime dt)
        {
            return ObjectUtils.GetEnumName<DniTygodnia>(dt.DayOfWeek);
        }

        public static bool IsEvenWeek(DateTime dt)
        {
            GregorianCalendar gc = new GregorianCalendar();

            int weekno = gc.GetWeekOfYear(dt, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Sunday);

            return (weekno % 2) == 0;
        }


        private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long CurrentTimeMillis()
        {
            return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }

        public static TimeSpan GetTimeSpan(double milliseconds)
        {
            return TimeSpan.FromMilliseconds(milliseconds);
        }
    }
}
