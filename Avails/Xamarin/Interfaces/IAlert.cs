using System.Threading.Tasks;

namespace Avails.Xamarin.Interfaces
{
    public interface IAlert
    {
        Task<string> Display(string title, string message, string firstButton, string secondButton, string cancel);
    }
}