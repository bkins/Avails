﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
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
        public static bool          WriteToOutput  { get; set; }
        public static bool          WriteToConsole { get; set; }
        public static bool          WriteToFile    { get; set; }
        public static bool          WriteToToast   { get; set; }
        public static bool          WriteToLogCat  { get; set; }
        public static bool          Verbose        { get; }
        public static string        FullLogPath    { get; }
        public static StringBuilder Log            { get; }
        public static string        CompleteLog
        {
            get => Log.ToString();
            private set { }
        }
        public static List<LogLine> LogList { get; set; }

        private static bool Ascending { get; set; }
        private static string Serialize(List<LogLine> list) => JsonConvert.SerializeObject(list);
        
        static Logger()
        {
            Log            = new StringBuilder();
            LogList        = new List<LogLine>();
            WriteToOutput  = false;
            WriteToConsole = false;
            WriteToFile    = true;
            WriteToToast   = false;
            WriteToLogCat  = false;
            Verbose        = false;
            Ascending      = true;
            FullLogPath    = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Logger.txt");

            var fileContents = GetFileContents();
            LogList   = Deserialize(fileContents);
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
                         , Category  = lineCategory
                         , Message   = lineMessage
                       } ).ToList();

            // return fileLines.Select(line => new LogLine { Message = line })
            //                 .ToList();
        }

        public static string ToggleLogListOrderByTimeStamp(string searchEditorText)
        {
            return Ascending ? 
                        ToStringOrderedByTimeStampDescending(searchEditorText) 
                      : ToStringOrderedByTimeStampAscending(searchEditorText);
        }

        private static string ToStringOrderedByTimeStampDescending(string searchEditorText)
        {
            Ascending = ! Ascending;
            
            var theList = ToListOrderedByTimeStampDescending(searchEditorText);

            return ListToString(theList.ToList());
        }

        private static string ToStringOrderedByTimeStampAscending(string searchEditorText)
        {
            Ascending = ! Ascending;
            
            var theList = ToListOrderedByTimeStampAscending(searchEditorText);

            return ListToString(theList.ToList());
        }

        private static IOrderedEnumerable<LogLine> ToListOrderedByTimeStampDescending(string searchEditorText)
        {
            if (searchEditorText.IsNullEmptyOrWhitespace())
            {
                return ToList().OrderByDescending(fields => fields.TimestampDateTime);
            }
            
            return ToList().Where( fields => fields.TimeStamp.Contains(searchEditorText, StringComparison.OrdinalIgnoreCase)
                                          || fields.Category.Contains(searchEditorText, StringComparison.OrdinalIgnoreCase)
                                          || fields.Message.Contains(searchEditorText, StringComparison.OrdinalIgnoreCase))
                           .OrderByDescending(fields => fields.TimestampDateTime);
        }

        private static IOrderedEnumerable<LogLine> ToListOrderedByTimeStampAscending(string searchEditorText)
        {
            if (searchEditorText.IsNullEmptyOrWhitespace())
            {
                return ToList().OrderBy(fields => fields.TimestampDateTime);
            }
            
            return ToList().Where( fields => fields.TimeStamp.Contains(searchEditorText, StringComparison.OrdinalIgnoreCase)
                                          || fields.Category.Contains(searchEditorText, StringComparison.OrdinalIgnoreCase)
                                          || fields.Message.Contains(searchEditorText, StringComparison.OrdinalIgnoreCase))
                           .OrderBy(fields => fields.TimestampDateTime);
        }

        private static List<LogLine> Deserialize(string json)
        {
            return json.IsValidJson() ? 
                          JsonConvert.DeserializeObject<List<LogLine>>(json) 
                        : LegacyLogFileToList(json);
        }

        public static List<LogLine> ToList()
        {
            return Deserialize(GetFileContents());
        }

        private static string ListToString(List<LogLine> list)
        {
            var log = new StringBuilder();

            foreach (var line in list)
            {
                log.AppendLine(line.ToString());
            }

            return log.ToString();
        }

        public static string GetFileContents()
        {
            return File.Exists(FullLogPath) ? 
                      File.ReadAllText(FullLogPath) 
                    : string.Empty;
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
                           Category  = category.ToString()
                         , Message   = message
                       };

            LogList.Add(line);

            return line;
        }

        private static void LogToToast(string completeLogMessage)
        {
            if ( ! WriteToToast) return;

            //BENDO: Implement for UWP 
            DependencyService.Get<IMessage>()
                             .ShortAlert(completeLogMessage);
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
                //ex = ex ?? new Exception();

                //BENDO:  Implement in Android (see PersonalTrainerWorkouts for example)
                //DependencyService.Get<IMessage>()
                //                 .Log(ConvertCategoryToLogLevel(category)
                //                    , completeLogMessage
                //                    , ex);
            }
        }

        private static string LogVerboseInfo(Exception ex)
        {
            string message;
            
            if (Verbose && ex != null)
            {
                message = $"{ex.Message}{Environment.NewLine}{ex.StackTrace}";
            }
            else
            {
                message = ex?.Message;
            }

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

        public static void Clear()
        {
            Log.Clear();
            CompleteLog = string.Empty;
            File.Delete(FullLogPath);
            File.Create(FullLogPath);
        }

        public static string SearchLog(string searchTerm)
        {
            var resultsList = LogList.Where(fields => fields.TimeStamp.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                                                   || fields.Category.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                                                   || fields.Message.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                                     .ToList();

            return ListToString(resultsList);
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
