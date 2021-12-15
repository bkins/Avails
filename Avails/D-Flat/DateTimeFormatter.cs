using System;

namespace Avails.D_Flat
{
    public static class DateTimeFormatter
    {
        public static string DateTimeTimeSpanForSaving(DateTime date
                                                     , TimeSpan time)
        {
            var displayTime    = DateTime.Today.Add(time).ToString("hh:mm tt");
            var dateTimeToSave = $"{date.ToShortDateString()} {displayTime}";
            
            return dateTimeToSave;
        }
    }
}
