using System;
using System.IO;
using SprintCrowdBackEnd.Enums;
using SprintCrowdBackEnd.Models;

namespace SprintCrowdBackEnd.Logger
{
    public class ScrowdLogger
    {
        public static AppSettings appSettings;
        public static void Log(string log, LogType logType = LogType.Info)
        {
            string formattedLog = BuildLogString(log);
            switch(logType)
            {
                case LogType.Info:
                    //friendly logs
                    PrintInfo(formattedLog);
                    break;
                case LogType.Warning:
                    //Warnings
                    PrintWarning(formattedLog);
                    break;
                case LogType.Error:
                    PrintError(formattedLog);
                    //Errors
                    break;
            }
        }

        private static void PrintInfo(string formattedLog)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(formattedLog);
            Console.ResetColor();
        }

        private static void PrintWarning(string formattedLog)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(formattedLog);
            Console.ResetColor();
            WriteLogToFile(formattedLog, LogType.Warning);
        }

        private static void PrintError(string formattedLog)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(formattedLog);
            Console.ResetColor();
            WriteLogToFile(formattedLog, LogType.Error);
        }
        
        /*
            Appends log string with date and time
         */
        private static string BuildLogString(string log)
        {
            //maybe append some more data in the future
            return $"{DateTime.UtcNow}: {log}";
        }

        private static void WriteLogToFile(string formattedLog, LogType logType)
        {
            try
            {
                using(StreamWriter handler = new StreamWriter(appSettings.Logging.LogPath, true))
                {
                    handler.WriteLine($"LogType: {logType.ToString()} => {formattedLog}");
                }
            }
            catch(IOException ex)
            {
                ScrowdLogger.Log($"Access to log file denied. Reason: {ex.Message}", LogType.Info);
            }

        }
    }
}