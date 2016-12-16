using System.Collections.Generic;
using NLog;
using SFA.DAS.Data.Pipeline;

namespace SFA.DAS.Data.AccountBalance
{
    public static class Logging
    {
        static Logging()
        {
            Logger = new LogFactory().GetLogger("default");
        }

        public static ILogger Logger { get; set; }


        private static readonly Dictionary<LoggingLevel,LogLevel> Xlate = 
            new Dictionary<LoggingLevel, LogLevel>
        {
            {LoggingLevel.Info, LogLevel.Info},
            {LoggingLevel.Debug, LogLevel.Debug },
            {LoggingLevel.Error, LogLevel.Error }
        };

        public static void Log(LoggingLevel level, string message)
        {
            Logger.Log(Xlate.ContainsKey(level) ? Xlate[level] : LogLevel.Info, message);
        }
    }
}