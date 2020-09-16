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

        public static IEnumerable<KeyValuePair<string, string>> GetMonthsList()
        {
            List<KeyValuePair<string, string>> months = new List<KeyValuePair<string, string>>();
            int i = 1;

            foreach (string monthName in DateTimeFormatInfo.CurrentInfo.MonthNames.Where(m => !string.IsNullOrEmpty(m)))
            {
                months.Add(new KeyValuePair<string, string>(i.ToString(), monthName));

                i++;
            }

            return months;
        }

        public static IEnumerable<DateTime> GetDaysOff(int year, int month, IEnumerable<DateTime> fixedHolidays)
        {
            List<DateTime> daysOff = new List<DateTime>();

            daysOff.AddRange(GetWeekends(year, month));
            daysOff.AddRange(fixedHolidays);
            daysOff.AddRange(GetMovingHolidays(year, month));

            return daysOff;
        }

        public static IEnumerable<DateTime> GetWeekends(int year, int month)
        {
            List<DateTime> weekends = new List<DateTime>();

            DateTime start = new DateTime(year, month, 1);
            DateTime end = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            for (DateTime d = start; d <= end; d = d.AddDays(1))
            {
                if (d.DayOfWeek == DayOfWeek.Saturday || d.DayOfWeek == DayOfWeek.Sunday)
                {
                    weekends.Add(d);
                }
            }

            return weekends;
        }

        public static IEnumerable<DateTime> GetMovingHolidays(int year, int month)
        {
            List<DateTime> holidays = new List<DateTime>();

            DateTime easter = GetEaster(year);
            holidays.AddRange(
                (new DateTime[] { easter, easter.AddDays(1), easter.AddDays(49), easter.AddDays(60) })
                .Where(d => d.Month == month)
                );

            return holidays;
        }

        private static DateTime GetEaster(int year)
        {
            int a = year % 19,
                b = year / 100,
                c = year % 100,
                d = b / 4,
                e = b % 4,
                f = (b + 8) / 25,
                g = (b - f + 1) / 3,
                h = (19 * a + b - d - g + 15) % 30,
                i = c / 4,
                k = c % 4,
                l = (32 + 2 * e + 2 * i - h - k) % 7,
                m = (a + 11 * h + 22 * l) / 451,
                month = (h + l - 7 * m + 114) / 31,
                day = ((h + l - 7 * m + 114) % 31) + 1;

            return new DateTime(year, month, day);
        }

        public static DateTime? FromUTCDate(DateTime? dt, int timezoneOffset)
        {
            //  Convert a DateTime (which might be null) from UTC timezone
            //  into the user's timezone. 
            if (dt == null)
                return null;

            DateTime newDate = dt.Value - new TimeSpan(timezoneOffset / 60, timezoneOffset % 60, 0);

            return newDate;
        }

        public static DateTime? ToUTCDate(DateTime? dt, int timezoneOffset)
        {
            //  Convert a DateTime (which might be null) from the user's timezone
            //  into UTC timezone. 
            if (dt == null)
                return null;

            DateTime newDate = dt.Value + new TimeSpan(timezoneOffset / 60, timezoneOffset % 60, 0);

            return newDate;
        }

        public static DateTime ApplyOffset(DateTime input, int timezoneOffsetMinutes)
        {
            //TimeSpan timezoneOffset = TimeSpan.FromMinutes(timezoneOffsetMinutes);

            //DateTimeOffset dateTimeOffset = new DateTimeOffset(dateTime).ToOffset(timezoneOffset);

            //return dateTimeOffset.DateTime;

            return input.AddMinutes(-timezoneOffsetMinutes);
        }

        public static DateTime DeleteOffset(DateTime input, int timezoneOffsetMinutes)
        {
            return input.AddMinutes(timezoneOffsetMinutes);
        }

        public static DateTime ApplyOffsetByTimezone(DateTime input, string timeZoneName)
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneName);

            var local = TimeZoneInfo.Local;

            return input.Add(timeZone.BaseUtcOffset - local.BaseUtcOffset);
        }

        /// <summary>
        /// Returns a UTC time in the user's specified timezone.
        /// </summary>
        /// <param name="utcTime">The utc time to convert</param>
        /// <param name="timeZoneName">Name of the timezone (Eastern Standard Time)</param>
        /// <returns>New local time</returns>
        public static DateTime GetUserTime(TimeZoneInfo timeZone, DateTime? utcTime = null)
        {
            if (utcTime == null)
                utcTime = DateTime.UtcNow;

            return TimeZoneInfo.ConvertTimeFromUtc(utcTime.Value, timeZone);
        }

        /// <summary>
        /// Converts local server time to the user's timezone and
        /// returns the UTC date.
        /// 
        /// Use this to convert user captured date inputs and convert
        /// them to UTC.  
        /// 
        /// User input (their local time) comes in as local server time 
        /// -> convert to user's timezone from server time
        /// -> convert to UTC
        /// </summary>
        /// <param name="localServerTime"></param>
        /// <returns></returns>
        public static DateTime GetUtcUserTime(DateTime? localServerTime, TimeZoneInfo timeZone)
        {
            if (localServerTime == null)
                localServerTime = DateTime.Now;

            return TimeZoneInfo.ConvertTime(localServerTime.Value, timeZone).ToUniversalTime();
        }

        public static Tuple<DateTime, DateTime> GetDateRange(DateTime date, int days, TimeZoneInfo timeZone)
        {
            var userDate = GetUserTime(timeZone, date);

            // force date boundary to be matched to users time
            var start = userDate.Date.AddDays(days * -1).ToUniversalTime();
            var end = userDate.Date.AddDays(1).AddSeconds(-1).ToUniversalTime();

            return Tuple.Create(start, end);
        }

        public static string DateMath(DateTime start, TimeZoneInfo timeZone)
        {
            var offset = timeZone.GetUtcOffset(start).TotalHours;
            var offsetLocal = TimeZoneInfo.Local.GetUtcOffset(start).TotalHours;
            var startTime = start.AddHours(offsetLocal - offset);

            // this is the time you write to the db
            var timeToSave = startTime.ToUniversalTime();

            return TimeZoneInfo.Local.ToString() + "<hr/>" +
                   "Captured time: " + start + " -> UTC:  " + start.ToUniversalTime() + "  <hr/>   " +
                   "Adjusted time: " + startTime + " -> UTC: " + timeToSave;
        }

        /// <summary>
        /// Converts a local machine time to the user's timezone time
        /// by applying the difference between the two timezones.
        /// </summary>
        /// <param name="localTime">local machine time</param>
        /// <param name="tzi">Timezone to adjust for</param>
        /// <returns></returns>
        public static DateTime AdjustTimeZoneOffset(DateTime localTime, TimeZoneInfo tzi = null)
        {  
            var offset = tzi.GetUtcOffset(localTime).TotalHours;
            var offset2 = TimeZoneInfo.Local.GetUtcOffset(localTime).TotalHours;

            return localTime.AddHours(offset2 - offset);
        }
    }
}
