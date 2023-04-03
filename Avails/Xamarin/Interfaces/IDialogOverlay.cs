using Xamarin.Forms;

namespace Avails.Xamarin.Interfaces;

public interface IDialogOverlay
{
    void Initialize(ContentPage loadingIndicatorPage = null);

    void Show();

    void Hide();
}