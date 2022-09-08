using System;
using Avails.D_Flat;
using Avails.Xamarin.Logger;
using Syncfusion.XForms.Buttons;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
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
            Logger.Logger.WriteLine("Test of information.", Category.Information);
            Logger.Logger.WriteLine("Test og Warning", Category.Warning);
            Logger.Logger.WriteLine("Test of Error", Category.Error);
            
            PageData      = new MessageLogViewModel();
            SearchOptions = new SearchOptions();
            
            InitializeComponent();
            
            BindingContext = PageData;
            
            LoadLogFile();
        }

        private void LoadLogFile()
        {
            Title            = GetTitleText();
            ShowSize.Text    = ShowLogSizeWarning;
            
            //string logContents = Logger.ToggleLogListOrderByTimeStampAsSting(SearchEditor.Text);

            SearchOptions.SearchTerm = SearchEditor.Text;
            
            // LogContents.Text = Logger.ToggleLogListOrderByTimeStampAsSting(SearchOptions);
            
            LogContents.HtmlText = Logger.Logger.ToggleLogListOrderByTimeStampAsSting(SearchOptions) ?? string.Empty;

        }

        private static string GetTitleText()
        {
            return $"Log size: {FileSizeFormatter.FormatSize(Logger.Logger.GetLogFileSizeInBytes())}";
        }

        private void ClearLogToolbarItem_OnClicked(object    sender
                                                 , EventArgs e)
        {
            Logger.Logger.Clear();
        }

        private void LogDescending_OnClicked(object    sender
                                           , EventArgs e)
        {
            LogContents.HtmlText = Logger.Logger.ToggleLogListOrderByTimeStampAsSting(SearchOptions);
        }

        private void SearchEditor_OnTextChanged(object               sender
                                              , TextChangedEventArgs e)
        {
            SearchOptions.SearchTerm = e.NewTextValue;
            LogContents.HtmlText     = Logger.Logger.SearchLog(SearchOptions);
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
            LogContents.HtmlText     = Logger.Logger.SearchLog(SearchOptions);
        }

        private void FilterWarningsCheckbox_OnStateChanged(object                sender
                                                         , StateChangedEventArgs e)
        {
            SearchOptions.ShowWarnings = FilterWarningsCheckbox.IsChecked ?? false;
            LogContents.HtmlText       = Logger.Logger.SearchLog(SearchOptions);
        }

        private void FilterInformationCheckbox_OnStateChanged(object                sender
                                                            , StateChangedEventArgs e)
        {
            SearchOptions.ShowInformation = FilterInformationCheckbox.IsChecked ?? false;
            LogContents.HtmlText          = Logger.Logger.SearchLog(SearchOptions);
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
