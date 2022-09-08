using System;

namespace Avails.Xamarin.Logger
{
    public class LogLine
    {
        public string   TimeStamp { get; set; }
        public Category Category  { get; set; }
        public string   Message   { get; set; }

        public DateTime TimestampDateTime
        {
            get => DateTime.Parse(TimeStamp);
        }

        public LogLine ()
        {
            TimeStamp = $"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}";
        }
        
        public string ToString(bool formatAsHtml = false)
        {
            return formatAsHtml 
                ? BuildLineAsHtml() 
                : $"{TimeStamp} | {Category.ToString()} | {Message}";
        }

        private string BuildLineAsHtml()
        {
            var timeStamp = $"<p style=\"color:gray\">{TimeStamp}</p>";
            var category  = Category.ToString() ?? string.Empty;

            category = Category switch
            {
                Category.Error => $"<p style=\"color:red\">{Category.ToString()}</p>"
              , Category.Warning => $"<p style=\"color:yellow\">{Category.ToString()}</p>"
              , Category.Information => $"<p style=\"color:green\">{Category.ToString()}</p>"
              , _ => category
            };

            return $"{category}{timeStamp}{Message}<hr style='margin-top:1.5em' />";
        }
    }
}