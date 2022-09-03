using System.Threading.Tasks;
using ApplicationExceptions;
using Avails.Xamarin.Interfaces;
using Xamarin.Forms;

// using Microsoft.Maui;
// using Microsoft.Maui.Controls;


namespace Avails.Xamarin
{
    public static class Alert
    {
        public static async Task<AlertResults> Display(string title
                                               , string message
                                               , string firstButton
                                               , string secondButton
                                               , string thirdButton)
        {
            var alert = DependencyService.Get<IAlert>();

            var result = await alert.Display(title
                                           , message
                                           , firstButton
                                           , secondButton
                                           , thirdButton);

            if (result == firstButton)  return AlertResults.First;
            if (result == secondButton) return AlertResults.Second;
            if (result == thirdButton)  return AlertResults.Third;

            throw new UnexpectedResultException("An unexpected result from the Alert Display was encountered.");
        }
    }

    public enum AlertResults
    {
        First
      , Second
      , Third
    }
}