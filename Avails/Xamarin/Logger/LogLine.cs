using System;
using Avails.D_Flat.Extensions;

namespace Avails.Xamarin.Logger
{
    public class LogLine
    {
        public string TimeStamp { get; set; }
        public string Category  { get; set; }
        public string Message   { get; set; }
        
        public DateTime TimestampDateTime
        {
            get => DateTime.Parse(TimeStamp);
        }

        public LogLine ()
        {
            TimeStamp = $"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}";
        }
        
        public override string ToString()
        {
            if (Category is null 
             || Category.IsNullEmptyOrWhitespace())
            {
                return Message;
            }
            
            return $"{TimeStamp} | {Category} | {Message}";
        }
    }
}