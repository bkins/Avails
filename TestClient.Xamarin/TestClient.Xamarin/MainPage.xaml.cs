using System;
using Avails.Xamarin;
using Avails.Xamarin.Logger;
using Avails.Xamarin.Views.LoggingPage;
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
            //These require implementing IMessage for UWP and Android
            Logger.WriteToLogCat = false;
            Logger.WriteToToast  = false;
            
            const string messageLogName = nameof(MessageLog);
            
            try
            {
                await PageNavigation.NavigateTo(messageLogName).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                Logger.WriteLine($"Error occured while trying to load {messageLogName}", Category.Error, exception);
            }
        }
    }
}