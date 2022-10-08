﻿using System;
using System.Reflection;
using Avails.D_Flat;
using Avails.Xamarin.Logger;
using Syncfusion.XForms.Buttons;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Log = Avails.Xamarin.Logger.Logger;
using static Avails.Xamarin.Configuration;

namespace Avails.Xamarin.Views.LoggingPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessageLog : ContentPage
    {
        public ImageSource image { get; set; }
        public ImageSource SearchImage { get; set; }

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
            
            SetDeleteImageSource("Delete.png");
            SetSearchImageSource("baseline_search_black_48.png"); 
            SetReorderButtonImage("baseline_swap_vert_black_18.png");
            
            InitializeComponent();
            
            BindingContext = PageData;
            
            LoadLogFile();
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
            Title            = GetTitleText();
            ShowSize.Text    = ShowLogSizeWarning;
            
            //string logContents = Logger.ToggleLogListOrderByTimeStampAsSting(SearchEditor.Text);

            SearchOptions.SearchTerm = SearchEditor.Text;
            
            // LogContents.Text = Logger.ToggleLogListOrderByTimeStampAsSting(SearchOptions);
            var          logContents       = Log.ToggleLogListOrderByTimeStampAsSting(SearchOptions);
            
            LogContents.HtmlText = logContents;
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