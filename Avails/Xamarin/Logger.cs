using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Avails.Xamarin.Interfaces;
using NLog;
using Xamarin.Forms;

namespace Avails.Xamarin
{
    public static class Logger
    {
        //BENDO: Implement a settings page to handle the setting of these values (or at least the ones that are appropriate)
        public static bool WriteToOutput  { get; set; }
        public static bool WriteToConsole { get; set; }
        public static bool WriteToFile    { get; set; }
        public static bool WriteToToast   { get; set; }
        public static bool WriteToLogCat  { get; set; }
        public static bool Verbose        { get; set; }
        
        public static string FullLogPath { get; set; }

        public static StringBuilder Log { get; }

        public static string CompleteLog => Log.ToString();

        static Logger()
        {
            Log = new StringBuilder();

            WriteToOutput  = false;
            WriteToConsole = false;
            WriteToFile    = true;
            WriteToToast   = false;
            WriteToLogCat  = false;
            Verbose        = false;

            FullLogPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Logger.txt");
        }

        public static void WriteLine(string    message
                                   , Category  category
                                   , Exception ex = null)
        {
            var completeLogMessage = $"[{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}] {category}: {message}{Environment.NewLine}";

            Log.AppendLine(completeLogMessage);

            LogVerboseInfo(ex);

            LogToLogCat(category
                      , ex
                      , completeLogMessage);

            LogToOutput(category
                      , completeLogMessage);

            LogToConsole(completeLogMessage);

            LogToFile(completeLogMessage);

            LogToToast(completeLogMessage);
        }

        private static void LogToToast(string completeLogMessage)
        {
            if (WriteToToast)
            {
                //BENDO: Implement for UWP 
                DependencyService.Get<IMessage>()
                                 .ShortAlert(completeLogMessage);
            }
        }

        private static void LogToFile(string completeLogMessage)
        {
            if (WriteToFile)
            {
                File.AppendAllText(FullLogPath, completeLogMessage);
            }
        }

        private static void LogToConsole(string completeLogMessage)
        {
            if (WriteToConsole)
            {
                Console.WriteLine(completeLogMessage);
            }
        }

        private static void LogToOutput(Category category
                                      , string   completeLogMessage)
        {
            if (WriteToOutput)
            {
                Debug.WriteLine(completeLogMessage
                              , category.ToString());
            }
        }

        private static void LogToLogCat(Category  category
                                      , Exception ex
                                      , string    completeLogMessage)
        {
            if (WriteToLogCat)
            {
                ex = ex ?? new Exception();

                //BENDO:  Implement in Android (see PersonalTrainerWorkouts for example)
                //DependencyService.Get<IMessage>()
                //                 .Log(ConvertCategoryToLogLevel(category)
                //                    , completeLogMessage
                //                    , ex);
            }
        }

        private static void LogVerboseInfo(Exception ex)
        {
            if (Verbose && ex != null)
            {
                Log.AppendLine(ex.StackTrace);
            }
        }

        public static LogLevel ConvertCategoryToLogLevel(Category category)
        {
            switch (category)
            {
                case Category.Information:

                    return LogLevel.Info;

                case Category.Error:

                    return LogLevel.Error;

                case Category.Warning:

                    return LogLevel.Warn;

                case Category.Unknown:
                default:

                    return LogLevel.Debug;
            }
        }

        public static Category ConvertLogLevelToCategory(LogLevel level)
        {
            switch (level.Name.ToLower())
            {
                case "info":

                    return Category.Information;

                case "error":

                    return Category.Error;

                case "warn":

                    return Category.Warning;

                default:

                    return Category.Unknown;
            }
        }

        public static void Clear()
        {
            Log.Clear();
        }
    }

    public enum Category
    {
        Error
      , Warning
      , Information
      , Unknown
    }
}
