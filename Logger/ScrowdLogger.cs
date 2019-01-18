using System;
using SprintCrowdBackEnd.Enums;

namespace SprintCrowdBackEnd.Logger
{
    public class ScrowdLogger
    {
        public static void Log(string log, LogType logType = LogType.Info)
        {
            switch(logType)
            {
                case LogType.Info:
                    //friendly logs
                    PrintInfo(log);
                    break;
                case LogType.Warning:
                    //Warnings
                    PrintWarning(log);
                    break;
                case LogType.Error:
                    PrintError(log);
                    //Errors should be handled, right now saving them to db
                    break;
            }
        }

        private static void PrintInfo(string log)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(BuildLogString(log));
            Console.ResetColor();
        }

        private static void PrintWarning(string log)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(BuildLogString(log));
            Console.ResetColor();
        }

        private static void PrintError(string log)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(BuildLogString(log));
            Console.ResetColor();  
        }
        
        /*
            Appends log string with date and time
         */
        private static string BuildLogString(string log)
        {
            //maybe append some more data in the future
            return $"{DateTime.UtcNow} : {log}";
        }
    }
}