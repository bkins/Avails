using System;
using System.Text;
using Avails.D_Flat.Extensions;

namespace Avails.Xamarin.Logger
{
    public class LogLine
    {
        public string TimeStampAsHtml
        {
            get => $"<p style=\"color:gray\">{TimeStamp}</p>";
        }

        public string TimeStamp { get; set; }

        public string CategoryAsHtml
        {
            get
            {
                var category = Category.ToString() ?? string.Empty;

                category = Category switch
                {
                    Category.Error => $"<p style=\"color:red\">{Category.ToString()}</p>"
                  , Category.Warning => $"<p style=\"color:yellow\">{Category.ToString()}</p>"
                  , Category.Information => $"<p style=\"color:green\">{Category.ToString()}</p>"
                  , _ => category
                };

                return category;
            }
        }

        public Category Category { get; set; }

        public string MessageAsHtml
        {
            get => $"<p style=\"color:gray\">{Message}</p>";
        }

        public string Message { get; set; }

        public DateTime TimestampDateTime
        {
            get => DateTime.Parse(TimeStamp);
        }

        public string ExtraDetails { get; set; }

        public LogLine ()
        {
            TimeStamp = $"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}";
        }
        
        public bool HasExtraDetails { get => ExtraDetails.HasValue(); }
        
        public string ToString(bool formatAsHtml = false)
        {
            var extraLine = string.Empty;

            if (ExtraDetails.HasValue())
            {
                // extraLine = $"{Environment.NewLine}{ExtraDetails}";
                extraLine = $"{ExtraDetails}";
            }
            return formatAsHtml 
                ? BuildLineAsHtml() 
                : $"{TimeStamp} | {Category.ToString()} | {Message}{extraLine}";
        }

        private string BuildLineAsHtml()
        {
            var timeStamp = StyleTextWithColor(TimeStamp, "gray"); //$"<p style=\"color:gray\">{TimeStamp}</p>";

            var color = Category switch
            {
                Category.Error => "red"
              , Category.Warning => "yellow"
              , Category.Information => "green"
              , _ => "black"
            };
            
            var category  = Category.ToString() is null 
                                        ? "Unknown" 
                                        : Category.ToString();

            category = StyleTextWithColor(category, color); //$"<p style=\"color:{color}\">{category}</p>";

            var message   = StyleTextWithColor(Message, "white"); //$"<p style=\"color:white\">{Message}</p>";
            var extraLine = string.Empty;

            if ( ! ExtraDetails.HasValue())
                return $"{category}{timeStamp}{message}{extraLine}<hr style='margin-top:1.5em' />";

            // extraLine = $"{Environment.NewLine}{ExtraDetails.Replace(@"\", @"\\")}";
            extraLine = $"{ExtraDetails.Replace(@"\", @"_")}";
            //extraLine = $"{extraLine.Replace(Environment.NewLine, "")}";
            extraLine = StyleTextWithColor(extraLine, "white");// $"<p style=\"color:white\">{extraLine}</p>";

            return $"{category}{timeStamp}{message}{extraLine}<hr style='margin-top:1.5em' />";
        }

        private string StyleTextWithColor(string text
                                        , string color)
        {
            var htmlLines = new StringBuilder();
            var lines = text.Split(Environment.NewLine.ToCharArray()
                                 , StringSplitOptions.None);
            
            foreach (var line in lines)
            {
                htmlLines.Append($"<p style=\"color:{color}\">{line}</p>");
            }
            // return $"<p style=\"color:{color}\">{text}</p>";
            return htmlLines.ToString();
        }
    }
}