using Org.BouncyCastle.Utilities;
using PDCore.Enums;
using PDCore.Extensions;
using PDCore.Interfaces;
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
            return EnumUtils.GetEnumName<DniTygodnia>(dt.DayOfWeek);
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

        public static long CurrentTimeSeconds()
        {
            return (long)(DateTime.UtcNow - Jan1st1970).TotalSeconds;
        }

        public static TimeSpan GetTimeSpan(double milliseconds)
        {
            return TimeSpan.FromMilliseconds(milliseconds);
        }

        public static int CalculateAge(DateTime birthdate) //08.08.1990
        {
            // Save today's date.
            var today = DateTime.Today; //20.01.2020

            // Calculate the age.
            var age = today.Year - birthdate.Year; //30

            // Go back to the year the person was born in case of a leap year
            if (birthdate.Date > today.AddYears(-age)) age--;

            return age;
        }

        public static int GetDaysUntilNextBirthday(DateTime birthDate)
        {
            var today = DateTime.Today;
            var birthday = new DateTime(today.Year, birthDate.Month, 1);
            birthday = birthday.AddDays(birthDate.Day - 1);

            if (birthday < today)
            {
                birthday = birthday.AddYears(1);
            }

            return (int)(birthday - today).TotalDays;
        }

        public static string GetISO860() => DateTime.UtcNow.ToISO8601();

        public static bool Exist(IPeriod period, IEnumerable<IPeriod> periods, DateComparison dateComparison)
        {
            return periods.Any(p => p != period && p.StartDate.CompareTo(period.StartDate) == (int)dateComparison);
        }

        public static TimeSpan GetTimeUntilNext(IPeriod period, IEnumerable<IPeriod> periods)
        {
            var next = periods
                .OrderBy(p => p.StartDate)
                .FirstOrDefault(p => p != period && p.StartDate >= period.StartDate);

            if (next == null)
                return TimeSpan.MinValue;

            return next.StartDate - period.GetEndDate();
        }

        public static DateTimeOffset ExtendContract(DateTimeOffset current, int months)
        {
            var newContractDate = current.AddMonths(months).AddTicks(-1);

            return new DateTimeOffset(newContractDate.Year,
                newContractDate.Month,
                DateTime.DaysInMonth(newContractDate.Year, newContractDate.Month),
                23,
                59,
                59,
                current.Offset);
        }

        // This presumes that weeks start with Monday.
        // Week 1 is the 1st week of the year with a Thursday in it.
        public static int GetIso8601WeekOfYear(DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);

            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public static DateTimeOffset FromUnixTimeSeconds(long seconds) => Jan1st1970.AddSeconds(seconds);

        public static DateTimeOffset FromUnixTimeMilliseconds(long milliseconds) => Jan1st1970.AddMilliseconds(milliseconds);

        public static long ToUnixTimeSeconds(DateTimeOffset dateTimeOffset) => (long)(dateTimeOffset - Jan1st1970).TotalSeconds;

        public static long ToUnixTimeMilliseconds(DateTimeOffset dateTimeOffset) => (long)(dateTimeOffset - Jan1st1970).TotalMilliseconds;
    }
}
