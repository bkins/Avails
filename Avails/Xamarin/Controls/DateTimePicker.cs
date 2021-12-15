using System;
using System.Collections.ObjectModel;
using System.Globalization;
using Syncfusion.SfPicker.XForms;

namespace Avails.Xamarin.Controls
{
    /// <summary>
    /// DateTime is the actual DataSource for SfPicker control which holds the collection of Date, Hour, Minute and Format
    /// </summary>
    public class DateTimePicker : SfPicker
    {
        public ObservableCollection<object> DateTime { get; set; }
        public ObservableCollection<object> Date;
        public ObservableCollection<object> Year;
        public ObservableCollection<object> Month;
        public ObservableCollection<object> Day;
        public ObservableCollection<object> Minute;
        public ObservableCollection<object> Hour;

        //AM and PM
        public ObservableCollection<object> Format;

        /// <summary>
        /// Header api holds the column name for every column in DateTime picker
        /// </summary>
        public ObservableCollection<string> Headers { get; set; }

        private static int YearNow  => System.DateTime.Now.Date.Year;
        private static int MonthNow => System.DateTime.Now.Date.Month;
        private static int DayNow   => System.DateTime.Now.Date.Day;

        public DateTimePicker()
        {
            DateTime = new ObservableCollection<object>();
            Date     = new ObservableCollection<object>();
            Year     = new ObservableCollection<object>();
            Month    = new ObservableCollection<object>();
            Day      = new ObservableCollection<object>();
            Minute   = new ObservableCollection<object>();
            Hour     = new ObservableCollection<object>();
            Format   = new ObservableCollection<object>();

            Headers = new ObservableCollection<string>
                      {
                          "Yr"
                        , "Mnth"
                        , "Day"
                        , "Hrs"
                        , "Min"
                        , "Format"
                      };

            PopulateCollections(DateTime);

            ItemsSource      = DateTime;
            ColumnHeaderText = Headers;
            
            HeaderText       = "Date Time Picker";
            ShowFooter       = true;
            ShowHeader       = true;
            ShowColumnHeader = true;
        }

        private void PopulateCollections(ObservableCollection<object> dateTime)
        {
            //Populate Year
            for (var year = 1900; year < YearNow + 100; year++)
            {
                Year.Add(year.ToString());
            }

            //Populate Month
            for (var month = 1; month <= 12; month++)
            {
                Month.Add(ShortMonth(month));
            }

            for (int day = 1; day <= 31; day++)
            {
                Day.Add(PadDay(day));
            }

            //Populate Date
            //var numberOfDays = System.DateTime
            //                         .DaysInMonth(YearNow, MonthNow);

            //for (var day = 1; day <= numberOfDays; day++)
            //{
            //    var date = day != DayNow ?
            //                       FormatDate(day) :
            //                       "Today";

            //    //Populate Day with Month
            //    Date.Add(date);
                
            //}

            //Populate Hour
            for (var hour = 1; hour <= 12; hour++)
            {
                Hour.Add(hour.ToString());
            }

            //Populate Minute
            for (var minute = 1; minute < 60; minute++)
            {
                Minute.Add(PadDay(minute));
            }

            //Populate Format
            Format.Add("AM");
            Format.Add("PM");

            //DateTime.Add(Date);
            DateTime.Add(Year);
            DateTime.Add(Month);
            DateTime.Add(Day);
            DateTime.Add(Hour);
            DateTime.Add(Minute);
            DateTime.Add(Format);
        }

        private static string FormatDate(int day)
        {
            var dayOfWeek = new DateTime(YearNow
                                       , MonthNow
                                       , day).DayOfWeek;

            var date = $"{ShortDayOfWeek(dayOfWeek)} {ShortMonth(MonthNow)} ";

            date = PadDay(day);

            return date;
        }

        private static string PadDay(int day)
        {
            string date = string.Empty;

            date += day < 10 ?
                            $"0{day}" :
                            day.ToString();

            return date;
        }

        private static string ShortMonth(int aMonth)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(aMonth).Substring(0, 3);
        }

        private static string ShortDayOfWeek(DayOfWeek name)
        {
            return name.ToString().Substring(0, 3);
        }
    }
}