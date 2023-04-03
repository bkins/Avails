using System;
using System.Reflection;
using Avails.D_Flat;
using Avails.Xamarin.Logger;
using Syncfusion.ListView.XForms;
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

        private bool PageLoading = true;
        
        public MessageLog()
        {
            if ( ! PageLoading) { return; }
            
            SearchOptions = new SearchOptions();
            
            try
            {
                InitializeComponent();
            }
            catch (Exception e)
            {
                Log.WriteLine($"Could not Initialize Components, because {e.Message}"
                            , Category.Error
                            , e);
            }
            
        }

        protected override void OnAppearing()
        {
            PageLoading = true;
            
            base.OnAppearing();
            
            LoadMessagePageData();
            
            // PageData      = new MessageLogViewModel();
            // SearchOptions = new SearchOptions();
            //
            // Title = GetTitleText();
            //
            // ShowSize.Text            = ShowLogSizeWarning;
            // SearchOptions.SearchTerm = SearchEditor.Text;
            //     
            // var logContents = Log.ToggleLogListOrderByTimeStampAsSting(SearchOptions);
            //
            // LogContents.HtmlText = logContents;
            //
            BindingContext = PageData;

            Log.Ascending        = true;
            
            ListView.IsVisible   = true;
            ListView.ItemsSource = Log.ToggleLogListOrderByTimeStamp(SearchOptions);
            
            PageLoading          = false;

            //
            // ListView.ItemsSource = PageData.LogAsList;
        }
        private void LoadMessagePageData()
        {
            PageData      = new MessageLogViewModel();
            SearchOptions = new SearchOptions();
            
            //These require implementing IMessage for UWP and Android
            Log.WriteToLogCat = false;
            Log.WriteToToast  = false;
            
            Title = GetTitleText();
            
            MainThread.BeginInvokeOnMainThread(() =>
            {
                ShowSize.Text            = ShowLogSizeWarning;
                SearchOptions.SearchTerm = SearchEditor.Text;
            });

            //string logContents = "Loading...";

            //
            // var task = Task.FromResult(Task.Run(async () =>
            // {
            //     
            // }));
            
            // logContents = Log.ToggleLogListOrderByTimeStampAsSting(SearchOptions);
            // MainThread.BeginInvokeOnMainThread(() =>
            // {
            //     LogContents.HtmlText = logContents;
            // });
            //
            //
            // task.ContinueWith(t =>
            // {
            //     
            // });
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

        private static string GetTitleText()
        {
            return $"Log size: {FileSizeFormatter.FormatSize(Log.GetLogFileSizeInBytes())}";
        }

        private void ClearLogToolbarItem_OnClicked(object    sender
                                                 , EventArgs e)
        {
            Log.Clear();
            ListView.ItemsSource = null;
            ListView.IsVisible   = false;
            
            Title = GetTitleText();
        }

        private void LogDescending_OnClicked(object    sender
                                           , EventArgs e)
        {
            if (PageLoading) { return; }
            
            ListView.ItemsSource = Log.ToggleLogListOrderByTimeStamp(SearchOptions);
        }

        private void SearchEditor_OnTextChanged(object               sender
                                              , TextChangedEventArgs e)
        {
            if (PageLoading) { return; }

            SearchOptions.SearchTerm = e.NewTextValue;

            ListView.ItemsSource = Log.SearchLogAsList(SearchOptions);
        }

        private string GetLogContents()
        {
            var logContents = Log.SearchLog(SearchOptions);
            
            // var logContents = string.Empty;
            // ThreadPool.QueueUserWorkItem(o => logContents = Log.SearchLog(SearchOptions));

            return logContents;
        }
        private void ShowSize_OnClicked(object    sender
                                      , EventArgs e)
        {
            if (PageLoading) { return; }

            NeverShowLogSizeWarning = ! NeverShowLogSizeWarning;
            ShowSize.Text           = ShowLogSizeWarning;
        }
        
        private void FilterErrorsCheckbox_OnStateChanged(object                sender
                                                       , StateChangedEventArgs e)
        {
            if (PageLoading) { return; }

            SearchOptions.ShowErrors = FilterErrorsCheckbox.IsChecked ?? false;
            ListView.ItemsSource     = Log.SearchLogAsList(SearchOptions);
        }

        private void FilterWarningsCheckbox_OnStateChanged(object                sender
                                                         , StateChangedEventArgs e)
        {
            if (PageLoading) { return; }

            SearchOptions.ShowWarnings = FilterWarningsCheckbox.IsChecked ?? false;
            ListView.ItemsSource       = Log.SearchLogAsList(SearchOptions);
        }

        private void FilterInformationCheckbox_OnStateChanged(object                sender
                                                            , StateChangedEventArgs e)
        {
            if (PageLoading) { return; }

            SearchOptions.ShowInformation = FilterInformationCheckbox.IsChecked ?? false;
            ListView.ItemsSource          = Log.SearchLogAsList(SearchOptions);
        }

        private void ShowSearchToolbarItem_OnClicked(object    sender
                                                   , EventArgs e)
        {
            if (PageLoading) { return; }
            
            SearchAndOrderGrid.IsVisible        = ! SearchAndOrderGrid.IsVisible;
            FilterErrorsCheckbox.IsVisible      = ! FilterErrorsCheckbox.IsVisible;
            FilterWarningsCheckbox.IsVisible    = ! FilterWarningsCheckbox.IsVisible;
            FilterInformationCheckbox.IsVisible = ! FilterInformationCheckbox.IsVisible;
        }

        private void ListView_OnSelectionChanged(object                        sender
                                               , ItemSelectionChangedEventArgs e)
        {
            var selected = (LogLine)ListView.SelectedItem;

            // if (selected?.ExtraDetails is null)
            //     return;
            //ExtraDetailRte.InsertHTMLText(selected.ToString(formatAsHtml: true));
            //ExtraDetailRte.HtmlText     = selected.ToString(formatAsHtml: true);
            //ExtraDetailRte.Text         = selected.ToString();
            // ExtraDetailRte.HtmlText         = selected.ToString(formatAsHtml: false);
            // ExtraDetailRte.DefaultFontColor = Color.White;
            ExtraDetailsEditor.Text      = selected.ToString(formatAsHtml: false);
            
            ListView.IsVisible           = false;
            ExtraDetailsEditor.IsVisible = true;
            DoneViewingButton.IsVisible  = true;

        }

        private void DoneViewingButton_OnClicked(object    sender
                                               , EventArgs e)
        {
            ListView.IsVisible          = true;
            //ExtraDetailRte.IsVisible    = false;
            ExtraDetailsEditor.IsVisible = false;
            DoneViewingButton.IsVisible  = false;
        }
    }
}
