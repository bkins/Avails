using System;

namespace Avails.D_Flat.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToShortDateTimeString(this DateTime value)
        {
            return $"{DateTime.Today.ToShortDateString()} {DateTime.Now.ToShortTimeString()}";
        }

        public static bool IsDateOnThisDayInThePast(this DateTime value)
        {
            return value.Date  <  DateTime.Today
                && value.Month == DateTime.Today.Month 
                && value.Day   == DateTime.Today.Day;
        }

        public static bool IsDateOnPassedInDate(this DateTime value
                                              , DateTime      dateTimeNow)
        {
            return value.Date.Year  <  dateTimeNow.Date.Year
                && value.Month      == dateTimeNow.Month 
                && value.Day        == dateTimeNow.Day;
        }
        
        /// <summary>
        /// Takes DateTime (value) and converts it to an eight digit integer: [yyyy][MM][dd].
        /// </summary>
        /// <param name="value">The dateTime to convert to eight digit integer.</param>
        /// <returns>An eight digit integer: [yyyy][MM][dd].</returns>
        public static int ToInt(this DateTime value)
        {
            var year  = value.Year.ToString();
            var month = value.Month.ToStringAsTwoDigits();
            var day   = value.Day.ToStringAsTwoDigits();

            return int.Parse($"{year}{month}{day}");
        }

        public static long ToLong(this DateTime value)
        {
            var date    = value.ToInt();
            var hours   = value.TimeOfDay.Hours.ToStringAsTwoDigits();
            var minutes = value.TimeOfDay.Minutes.ToStringAsTwoDigits();
            var seconds = value.TimeOfDay.Seconds.ToStringAsTwoDigits();

            return long.Parse($"{date}{hours}{minutes}{seconds}");
        }

        
    }
}
