using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avails.D_Flat.Extensions;
using Avails.Xamarin.Interfaces;
using Newtonsoft.Json;
using NLog;
using Xamarin.Forms;

//using Microsoft.Maui.Controls;

namespace Avails.Xamarin.Logger
{
    public static class Logger
    {
        //BENDO: Implement a settings page to handle the setting of these values (or at least the ones that are appropriate)
        public static  bool          WriteToOutput               { get; set; }
        public static  bool          WriteToConsole              { get; set; }
        public static  bool          WriteToFile                 { get; set; }
        public static  bool          WriteToToast                { get; set; }
        private static bool          WriteLineToToastForcedNotAllowed { get; set; }
        public static  bool          WriteToLogCat               { get; set; }
        public static  bool          Verbose                     { get; set; }
        public static  string        FullLogPath                 { get; }
        public static  StringBuilder Log                         { get; }
        public static string        CompleteLog
        {
            get => Log.ToString();
            private set { }
        }
        public static List<LogLine> LogList { get; set; }

        private static bool Ascending { get; set; }
        private static string Serialize(List<LogLine> list) => JsonConvert.SerializeObject(list);

        private const string LogIsEmpty = "Log is empty or there are not entries that match your search criteria.";
        
        static Logger()
        {
            LogList                     = new List<LogLine>();
            WriteToOutput               = false;
            WriteToConsole              = false;
            WriteToFile                 = true;
            WriteToToast                = false;
            WriteToLogCat               = true;
            WriteLineToToastForcedNotAllowed = false;
            Verbose                     = false;
            Ascending                   = true;
            FullLogPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder
                                                                            .LocalApplicationData)
                                     , "Logger.txt");

            Log         = new StringBuilder(GetFileContents());
            CompleteLog = Log.ToString();
            LogList     = Deserialize(CompleteLog);
        }

        private static List<LogLine> LegacyLogFileToList(string fileContents)
        {
            var fileLines = new List<string>(fileContents.Split(new [] { Environment.NewLine }
                                                              , StringSplitOptions.RemoveEmptyEntries));

            return (
                from line in fileLines
                select line.Split(']')
                into lineArray
                let lineTimeStamp = lineArray[0]
                                    .Replace("["
                                           , "")
                                    .Trim()
                let categoryMessage = lineArray[1].Split(':')
                let lineCategory = categoryMessage[0].Trim()
                let lineMessage = categoryMessage[1].Trim()
                select new LogLine
                       {
                           TimeStamp = lineTimeStamp
                         , Category  = GetEnum(lineCategory)
                         , Message   = lineMessage
                       } ).ToList();

            // return fileLines.Select(line => new LogLine { Message = line })
            //                 .ToList();
        }

        public static string ToggleLogListOrderByTimeStampAsSting(SearchOptions options)
        {
            
            var logAsString = Ascending
                ? ToStringOrderedByTimeStampDescending(options)
                : ToStringOrderedByTimeStampAscending(options);
            
            return logAsString;
        }

        public static IOrderedEnumerable<LogLine> ToggleLogListOrderByTimeStamp(SearchOptions options)
        {
            return Ascending
                ? ToListOrderedByTimeStampDescending(options)
                : ToListOrderedByTimeStampAscending(options);
        }
        
        private static string ToStringOrderedByTimeStampDescending(SearchOptions options)
        {
            Ascending = ! Ascending;
            
            var theList = ToListOrderedByTimeStampDescending(options);

            return ListToString(theList.ToList());
        }

        private static string ToStringOrderedByTimeStampAscending(SearchOptions options)
        {
            Ascending = ! Ascending;
            
            var theList = ToListOrderedByTimeStampAscending(options);

            return ListToString(theList.ToList());
        }

        private static IOrderedEnumerable<LogLine> ToListOrderedByTimeStampDescending(SearchOptions options)
        {
            if (options.SearchTerm.IsNullEmptyOrWhitespace())
            {
                return ToList().Where(  fields => FilterOptionsByCategory(options, fields))
                               .OrderByDescending( fields => fields.TimestampDateTime);
            }
            
            return ToList().Where(  fields => FilterBySearchTerm(options, fields)
                                           && FilterOptionsByCategory(options, fields))
                           .OrderByDescending(fields => fields.TimestampDateTime);
        }

        private static bool FilterBySearchTerm(SearchOptions options
                                             , LogLine       fields)
        {
            return (   fields.TimeStamp.Contains(options.SearchTerm
                                               , StringComparison.OrdinalIgnoreCase)
                    || fields.Category.ToString().Contains(options.SearchTerm
                                              , StringComparison.OrdinalIgnoreCase)
                    || fields.Message.Contains(options.SearchTerm
                                             , StringComparison.OrdinalIgnoreCase) );
        }

        private static bool FilterOptionsByCategory(SearchOptions options
                                                  , LogLine       fields)
        {
            return ( fields.Category == Category.Error && options.ShowErrors )
                || ( fields.Category == Category.Warning && options.ShowWarnings )
                || ( fields.Category == Category.Information && options.ShowInformation );
        }

        private static IOrderedEnumerable<LogLine> ToListOrderedByTimeStampAscending(SearchOptions options)
        {
            if (options.SearchTerm.IsNullEmptyOrWhitespace())
            {
                return ToList().Where(fields=>FilterOptionsByCategory(options, fields))
                               .OrderBy(fields => fields.TimestampDateTime);
            }
            
            return ToList().Where( fields => FilterBySearchTerm(options, fields)
                                   && FilterOptionsByCategory(options, fields))
                           .OrderBy(fields => fields.TimestampDateTime);
        }

        private static List<LogLine> Deserialize(string json)
        {
            return json.IsValidJson() 
                        ? JsonConvert.DeserializeObject<List<LogLine>>(json) 
                        : LegacyLogFileToList(json);
        }

        public static List<LogLine> ToList(bool forceRefresh = false)
        {
            if ( ! forceRefresh) { return LogList; }
            
            var task = Task.Factory.StartNew(() => Deserialize(GetFileContents()));
            
            Task.WaitAll();
            
            return task.Result;
            
            //return Deserialize(GetFileContents());
        }

        private static string ListToString(List<LogLine> list)
        {
            var log = new StringBuilder();

            foreach (var line in list)
            {
                log.AppendLine(line.ToString(true));
            }

            return log.Length == 0 
                        ? LogIsEmpty
                        : log.ToString();
        }

        public static string GetFileContents()
        {
            string fileContents;

            try
            {
                fileContents= File.Exists(FullLogPath) 
                                ? File.ReadAllText(FullLogPath) 
                                : string.Empty;
            }
            catch (Exception e)
            {
                WriteLine(e.Message, Category.Error, e);
                return string.Empty;
            }

            return fileContents;
        }

        public static long GetLogFileSizeInBytes()
        {
            if ( ! File.Exists(FullLogPath)) return 0;

            var fileInfo = new FileInfo(FullLogPath);

            return fileInfo.Length;
        }
        
        public static void WriteLineToToastForced(string    message
                                                , Category  category
                                                , Exception ex = null)
        {
            if (WriteLineToToastForcedNotAllowed)
            {
                WriteLine($"An attempt was made to force the log message to Toast, but is not allowed. {nameof(WriteLineToToastForcedNotAllowed)}: {WriteLineToToastForcedNotAllowed}"
                        , Category.Warning);
                return;
            }
            var setting = WriteToToast;

            WriteToToast = true;

            WriteLine(message
                    , category
                    , ex);

            WriteToToast = setting;
        }

        public static void WriteLine(string    message
                                   , Category  category
                                   , Exception ex = null)
        {
            var exceptionMessage = string.Empty;
            
            IntraAppCommunication.Instance.StringValue = message;
            
            if (ex != null)
            {
                exceptionMessage = LogVerboseInfo(ex);
                message          = $"{message}{Environment.NewLine}{exceptionMessage}";
            }

            var line = AddToLogList(message
                                  , category);

            var completeLogMessage = line.ToString();

            Log.AppendLine(completeLogMessage);

            LogToLogCat(category
                      , exceptionMessage
                      , completeLogMessage);

            LogToOutput(category
                      , completeLogMessage);

            LogToConsole(completeLogMessage
                       , category);

            LogToFile();

            LogToToast(message);
        }

        private static LogLine AddToLogList(string   message
                                          , Category category)
        {
            var line = new LogLine
                       {
                           Category  = category
                         , Message   = message
                       };

            LogList.Add(line);

            return line;
        }

        private static void LogToToast(string completeLogMessage)
        {
            if ( ! WriteToToast) return;

            try
            {
                DependencyService.Get<IMessage>()
                                 .ShortAlert(completeLogMessage);
            }
            catch (Exception e)
            {
                WriteToToast                     = false;
                WriteLineToToastForcedNotAllowed = true;
                
                WriteLine($"Could send previous message to toast, because: {e.Message}"
                        , Category.Error
                        , e);
                
                Console.WriteLine(e);
                
            }
        }

        private static void LogToFile()
        {
            if ( ! WriteToFile) return;

            var currentlyLogged   = GetFileContents();
            var currentLoggedList = JsonConvert.DeserializeObject<List<LogLine>>(currentlyLogged) ?? new List<LogLine>();

            currentLoggedList.AddRange(LogList);

            File.WriteAllText(FullLogPath, Serialize(LogList));
        }

        private static void LogToConsole(string   completeLogMessage
                                       , Category category)
        {
            if ( ! WriteToConsole) return;

            var initialForeColor = Console.ForegroundColor;

            SetConsoleForeColor(category
                              , initialForeColor);

            Console.WriteLine(completeLogMessage);

            Console.ForegroundColor = initialForeColor;
        }

        private static void SetConsoleForeColor(Category     category
                                              , ConsoleColor initialForeColor)
        {
            switch (category)
            {
                case Category.Error:

                    Console.ForegroundColor = ConsoleColor.Red;

                    break;

                case Category.Warning:

                    Console.ForegroundColor = ConsoleColor.Yellow;

                    break;

                case Category.Information:

                    Console.ForegroundColor = ConsoleColor.Green;

                    break;

                case Category.Unknown:

                    Console.ForegroundColor = ConsoleColor.Gray;

                    break;

                default:

                    Console.ForegroundColor = initialForeColor;

                    break;
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

        private static void LogToLogCat(Category category
                                      , string   exceptionMessage
                                      , string   completeLogMessage)
        {
            if (WriteToLogCat)
            {
                try
                {
                    DependencyService.Get<IMessage>()
                                     .Log(ConvertCategoryToLogLevel(category)
                                        , completeLogMessage
                                        , new Exception(exceptionMessage));
                }
                catch (Exception e)
                {
                    WriteToLogCat = false;

                    WriteLine($"An attempt to log to the Android LogCat failed, becuase: {e.Message}"
                            , Category.Error
                            , e);
                    
                    Console.WriteLine(e);
                }
            }
        }

        private static string LogVerboseInfo(Exception ex)
        {
            if (ex.InnerException != null)
            {
                LogVerboseInfo(ex.InnerException);
            }

            var message = Verbose 
                ? $"{ex.Message}{Environment.NewLine}{ex.StackTrace}" 
                : ex?.Message;

            Log.AppendLine(message);

            return message;
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

        public static string Clear()
        {
            Log.Clear();
            
            CompleteLog = string.Empty;
            
            File.Delete(FullLogPath);
            File.Create(FullLogPath);

            return LogIsEmpty;
        }

        public static string SearchLog(SearchOptions options)
        {

            var resultsList = LogList.Where(fields => FilterBySearchTerm(options
                                                                       , fields)
                                                   && FilterOptionsByCategory(options
                                                                            , fields))
                                     .ToList();

            return ListToString(resultsList);
        }
        
        public static Category GetEnum(string enumName)
        {
            return enumName switch
            {
                nameof(Category.Error) => Category.Error
              , nameof(Category.Warning) => Category.Warning
              , nameof(Category.Information) => Category.Information
              , _ => Category.Unknown
            };
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
