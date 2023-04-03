using System;
using System.Collections.Generic;
using System.Linq;

namespace Avails.D_Flat.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToShortDateTimeString(this DateTime value)
        {
            return $"{value.ToShortDateString()} {value.ToShortTimeString()}";
        }
        
        public static string NowShortDateTimeString(this DateTime value)
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

#region RelativeTimeSpan to words

        private static Dictionary<double, Func<double, string>> _instance;

        private static Dictionary<double, Func<double, string>> RelativitySetup()
        {
            var dict = new Dictionary<double, Func<double, string>>();
            dict.Add(0.75,            _ => "less than a minute");
            dict.Add(1.5,             _ => "about a minute");
            dict.Add(45,              _ => $"{Math.Round(_)} minutes");
            dict.Add(90,              _ => "about an hour");
            dict.Add(1440,            _ => $"about {Math.Round(Math.Abs(_ / 60))} hours"); // 60 * 24
            dict.Add(2880,            _ => "a day"); // 60 * 48
            dict.Add(43200,           _ => $"{Math.Floor(Math.Abs(_ / 1440))} days"); // 60 * 24 * 30
            dict.Add(86400,           _ => "about a month"); // 60 * 24 * 60
            dict.Add(525600,          _ => $"{Math.Floor(Math.Abs(_ / 43200))} months"); // 60 * 24 * 365 
            dict.Add(1051200,         _ => "about a year"); // 60 * 24 * 365 * 2
            dict.Add(double.MaxValue, _ => $"{Math.Floor(Math.Abs(_ / 525600))} years");

            return dict;
        }

        public static string ToRelativeDate(this DateTime input)
        {
            var fromNowSpan  = DateTime.Now.Subtract(input);
            var totalMinutes = fromNowSpan.TotalMinutes;
            var suffix       = " ago";

            if (totalMinutes < 0.0)
            {
                totalMinutes = Math.Abs(totalMinutes);
                suffix       = " from now";
            }

            _instance ??= RelativitySetup();

            return $"{_instance.First(span => totalMinutes < span.Key).Value.Invoke(totalMinutes)}{suffix}";
        }

#endregion
    }
}
