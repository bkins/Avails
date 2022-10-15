using Avails.Xamarin.Views.LoggingPage;
using Xamarin.Forms;

namespace TestClient.Xamarin
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(MessageLog), typeof(MessageLog));

        }
    }
}