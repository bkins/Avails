
using Xamarin.Essentials;

namespace Avails.Xamarin
{
    public static class Configuration
    {

        public static bool UseHtmlForEmailBody
        {
            get => Preferences.Get(nameof(UseHtmlForEmailBody)
                                 , false);
            set => Preferences.Set(nameof(UseHtmlForEmailBody)
                                 , value);
        }
        
        public static string EmailToDoctor
        {
            get => Preferences.Get(nameof(EmailToDoctor)
                                 , string.Empty);
            set => Preferences.Set(nameof(EmailToDoctor)
                                 , value);
        }

        public static string BackgroundImage
        {
            get => Preferences.Get(nameof(BackgroundImage)
                                 , string.Empty);
            set => Preferences.Set(nameof(BackgroundImage)
                                 , value);
        }

        public static string ZoeColor
        {
            get => Preferences.Get(nameof(ZoeColor)
                                 , "OriginalPrimary");
            set => Preferences.Set(nameof(ZoeColor)
                                 , value);
        }

        public static int Usage
        {
            get => Preferences.Get(nameof(Usage)
                                 , 0);
            set => Preferences.Set(nameof(Usage)
                                 , value);
        }

        public static bool NeverShowLogSizeWarning
        {
            get => Preferences.Get(nameof(NeverShowLogSizeWarning)
                                 , false);
            set => Preferences.Set(nameof(NeverShowLogSizeWarning)
                                 , value);
        }
    }
    
}
