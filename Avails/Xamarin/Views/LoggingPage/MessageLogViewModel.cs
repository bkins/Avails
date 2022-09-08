using System.Collections.ObjectModel;
using Avails.Xamarin.Logger;

namespace Avails.Xamarin.Views.LoggingPage
{
    public class MessageLogViewModel
    {
        public string                        CompleteLog       => Logger.Logger.CompleteLog;
        public ObservableCollection<LogLine> LogAsList         => new ObservableCollection<LogLine>(Logger.Logger.ToList());
        public bool                          ShowSearchOptions { get; set; }
    }
}
