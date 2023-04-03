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

        public static double RequestedEntryTextHeight
        {
            get => Preferences.Get(nameof(RequestedEntryTextHeight)
                                 , 25.0);
            set => Preferences.Set(nameof(RequestedEntryTextHeight)
                                 , value);
        }

        public static double Latitude
        {
            get => Preferences.Get(nameof(Latitude)
                                 , 47.6062d);
            set => Preferences.Set(nameof(Latitude)
                                 , value);
        }
        
        public static double Longitude
        {
            get => Preferences.Get(nameof(Longitude)
                                 , -122.3321d);
            set => Preferences.Set(nameof(Longitude)
                                 , value);
        }
        
        #region Notifications

        public static bool IsNotificationSettingsOn
        {
            get => Preferences.Get(nameof(IsNotificationSettingsOn)
                                        , false);
            set => Preferences.Set(nameof(IsNotificationSettingsOn)
                                 , value);
        }
        
        public static int NotificationInterval
        {
            get => Preferences.Get(nameof(NotificationSettings.Interval)
                                 , 0);
            set => Preferences.Set(nameof(NotificationSettings.Interval)
                                 , value);
        }

        public static string NotificationTitle
        {
            get => Preferences.Get(nameof(NotificationSettings.Title)
                                 , string.Empty);
            set => Preferences.Set(nameof(NotificationSettings.Title)
                                 , value);
        }

        public static string NotificationMessage
        {
            get => Preferences.Get(nameof(NotificationSettings.Message)
                                 , string.Empty);
            set => Preferences.Set(nameof(NotificationSettings.Message)
                                 , value);
        }

        public static int NotificationIntentId
        {
            get => Preferences.Get(nameof(NotificationSettings.IntentId), 0);
            set => Preferences.Set(nameof(NotificationSettings.IntentId)
                                 , value);
        }

        public static int SunsetIntentId
        {
            get => Preferences.Get(nameof(SunsetIntentId)
                                 , 0);
            set => Preferences.Set(nameof(SunsetIntentId)
                                 , value);
        }
        
        public static int SunriseIntentId
        {
            get => Preferences.Get(nameof(SunriseIntentId)
                                 , 0);
            set => Preferences.Set(nameof(SunriseIntentId)
                                 , value);
        }

        private struct NotificationSettings
        {
            public static string Message  { get; set; }
            public static string Title    { get; set; }
            public static int    Interval { get; set; }
            public static int    IntentId { get; set; }

        }
        
        #endregion

        #region DeleteOptions

        private static int _DeleteOption
        {
            get => Preferences.Get(nameof(_DeleteOption)
                                 , 0);
            set => Preferences.Set(nameof(_DeleteOption)
                                 , value);
            
        }

        public static DeleteOptions DeleteOption
        {
            get => (DeleteOptions) _DeleteOption;
            set => _DeleteOption = (int)value;
        }

        public enum DeleteOptions
        {
            Delete
          , Archive
          , Trash
          , Ask
        }

        #endregion
    }

}


