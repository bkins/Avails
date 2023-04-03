using System.Collections.ObjectModel;
using Avails.Xamarin.Logger;

namespace Avails.Xamarin.Views.LoggingPage
{
    public class MessageLogViewModel
    {
        public string                        CompleteLog       { get => Logger.Logger.CompleteLog; }
        public ObservableCollection<LogLine> LogAsList         { get => new (Logger.Logger.ToList()); }
        public bool                          ShowSearchOptions { get; set; }
    }
}
