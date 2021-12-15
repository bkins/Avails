using System;
using NLog;

namespace Avails.Xamarin.Interfaces
{
    public interface IMessage
    {
        
        void LongAlert(string  message);
        void ShortAlert(string message);

        void Log(LogLevel  level
               , string    message
               , Exception ex);
    }
}
