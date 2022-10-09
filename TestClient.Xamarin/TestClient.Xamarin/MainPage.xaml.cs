using System;
using Avails.Xamarin;
using Avails.Xamarin.Logger;
using Xamarin.Forms;

namespace TestClient.Xamarin
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void MessageLogButton_OnClicked(object    sender
                                              , EventArgs e)
        {
            var messageLagPageName = nameof(Avails.Xamarin.Views.LoggingPage.MessageLog);
            try
            {
                await PageNavigation.NavigateTo(messageLagPageName);
            }
            catch (Exception exception)
            {
                Logger.WriteLine($"Error occured while trying to load {messageLagPageName}", Category.Error, exception);
            }
        }
    }
}