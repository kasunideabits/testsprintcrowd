using System;
using System.IO;
using Serilog;
using Serilog.Events;
using SprintCrowdBackEnd.Enums;
using SprintCrowdBackEnd.Models;

namespace SprintCrowdBackEnd.Logger
{
    public class ScrowdLogger
    {
        private static ILogger logger;
        public static AppSettings appSettings;

        public static void InitLogger()
        {
            logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(
                    appSettings.Logging.LogPath,
                    fileSizeLimitBytes: 1_000_000,
                    rollOnFileSizeLimit: true,
                    shared: true,
                    flushToDiskInterval: TimeSpan.FromSeconds(1))
                .CreateLogger();
        }

        public static void Log(string log, LogType logType = LogType.Info)
        {
            switch(logType)
            {
                case LogType.Info:
                    //friendly logs
                    logger.Information(log);
                    break;
                case LogType.Warning:
                    //Warnings
                    logger.Warning(log);
                    break;
                case LogType.Error:
                    logger.Error(log);
                    //Errors
                    break;
            }
        }
    }
}