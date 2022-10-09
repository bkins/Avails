using System;
using System.Reflection;
using System.Threading;
using Avails.D_Flat;
using Avails.Xamarin.Logger;
using Syncfusion.XForms.Buttons;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Log = Avails.Xamarin.Logger.Logger;
using static Avails.Xamarin.Configuration;

namespace Avails.Xamarin.Views.LoggingPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessageLog : ContentPage
    {
        private MessageLogViewModel PageData      { get; set; }
        private SearchOptions       SearchOptions { get; set; }
        
        private string ShowLogSizeWarning
        {
            get => NeverShowLogSizeWarning ? "s" : "S";
        }

        public MessageLog()
        {
            PageData      = new MessageLogViewModel();
            SearchOptions = new SearchOptions();
            
            //These require implementing IMessage for UWP and Android
            Log.WriteToLogCat = false;
            Log.WriteToToast  = false;
            
            Log.WriteLine("Loading MessageLog page...", Category.Information);
            
            // SetDeleteImageSource("Avails/Xamarin/Resources/Drawable/Delete.png");
            // SetSearchImageSource("Avails/Xamarin/Resources/Drawable/baseline_search_black_48.png"); 
            // SetReorderButtonImage("Avails/Xamarin/Resources/Drawable/baseline_swap_vert_black_18.png");
            //
            InitializeComponent();
            
            BindingContext = PageData;
            
            Log.WriteLine("MessageLog page is loaded.", Category.Information);
            Log.WriteLine($"Right now is: {DateTime.Now.ToLongDateString()} at {DateTime.Now.ToLongTimeString()}"
                        , Category.Information);

            ThreadPool.QueueUserWorkItem(o => LoadLogFile());
            
            //LoadLogFile();

        }

        public void SetDeleteImageSource(string imageName)
        {
            ClearLogToolbarItem.IconImageSource = GetImageSource(imageName);
        }

        public void SetSearchImageSource(string imageName)
        {
            ShowSearchToolbarItem.IconImageSource = GetImageSource(imageName);
        }

        public void SetReorderButtonImage(string imageName)
        {
            ReorderButton.Source = GetImageSource(imageName);
        }
        
        private ImageSource GetImageSource(string imageName)
        {
            return ImageSource.FromResource(imageName
                                          , typeof(MessageLog).GetTypeInfo()
                                                              .Assembly);
        }
        private void LoadLogFile()
        {
            Title = GetTitleText();
            
            MainThread.BeginInvokeOnMainThread(() =>
            {
                ShowSize.Text            = ShowLogSizeWarning;
                SearchOptions.SearchTerm = SearchEditor.Text;
            });
            
            var logContents = Log.ToggleLogListOrderByTimeStampAsSting(SearchOptions);
            MainThread.BeginInvokeOnMainThread(() => { LogContents.HtmlText = logContents; });
        }

        private static string GetTitleText()
        {
            return $"Log size: {FileSizeFormatter.FormatSize(Log.GetLogFileSizeInBytes())}";
        }

        private void ClearLogToolbarItem_OnClicked(object    sender
                                                 , EventArgs e)
        {
            LogContents.HtmlText = Log.Clear();
        }

        private void LogDescending_OnClicked(object    sender
                                           , EventArgs e)
        {
            LogContents.HtmlText = Log.ToggleLogListOrderByTimeStampAsSting(SearchOptions);
        }

        private void SearchEditor_OnTextChanged(object               sender
                                              , TextChangedEventArgs e)
        {
            SearchOptions.SearchTerm = e.NewTextValue;
            LogContents.HtmlText     = Log.SearchLog(SearchOptions);
        }

        private void ShowSize_OnClicked(object    sender
                                      , EventArgs e)
        {
            NeverShowLogSizeWarning = ! NeverShowLogSizeWarning;
            ShowSize.Text           = ShowLogSizeWarning;
        }
        
        private void FilterErrorsCheckbox_OnStateChanged(object                sender
                                                       , StateChangedEventArgs e)
        {
            SearchOptions.ShowErrors = FilterErrorsCheckbox.IsChecked ?? false;
            LogContents.HtmlText     = Log.SearchLog(SearchOptions);
        }

        private void FilterWarningsCheckbox_OnStateChanged(object                sender
                                                         , StateChangedEventArgs e)
        {
            SearchOptions.ShowWarnings = FilterWarningsCheckbox.IsChecked ?? false;
            LogContents.HtmlText       = Log.SearchLog(SearchOptions);
        }

        private void FilterInformationCheckbox_OnStateChanged(object                sender
                                                            , StateChangedEventArgs e)
        {
            SearchOptions.ShowInformation = FilterInformationCheckbox.IsChecked ?? false;
            LogContents.HtmlText          = Log.SearchLog(SearchOptions);
        }

        private void ShowSearchToolbarItem_OnClicked(object    sender
                                                   , EventArgs e)
        {
            PageData.ShowSearchOptions          = ! PageData.ShowSearchOptions;
            
            SearchAndOrderGrid.IsVisible        = PageData.ShowSearchOptions;
            FilterErrorsCheckbox.IsVisible      = PageData.ShowSearchOptions;
            FilterWarningsCheckbox.IsVisible    = PageData.ShowSearchOptions;
            FilterInformationCheckbox.IsVisible = PageData.ShowSearchOptions;
        }
    }
}
