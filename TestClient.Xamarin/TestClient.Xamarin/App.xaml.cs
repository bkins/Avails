using Syncfusion.Licensing;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace TestClient.Xamarin
{
    public partial class App : Application
    {
        public App()
        {
            SyncfusionLicenseProvider.RegisterLicense(
                "NjgyNzUzQDMyMzAyZTMyMmUzMEg4dER0RGZ1L0IwWmFQc3lQZ1llOFE1bFpUNEcxa0xTNUFRbmJRR1FIMFU9");
            
            InitializeComponent();

            MainPage = new AppShell();

        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}