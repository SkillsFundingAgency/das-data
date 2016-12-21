using System;
using System.Collections.Generic;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using NLog;
using SFA.DAS.Data.Pipeline;
using LogLevel = NLog.LogLevel;

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

    //get rid of this and use centralised logging
    public class StorageLogging
    {
        public class LogMessage : TableEntity
        {
            public LogMessage()
            {
                PartitionKey = "logs";
                RowKey = Guid.NewGuid().ToString();
            }

            public LoggingLevel Level { get; set; }
            public string Message { get; set; }
        }

        public static void StorageLog(LoggingLevel level, string message)
        {
            var storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("Storage"));
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference("Logs");
            table.CreateIfNotExists();
            var insertOperation =
                TableOperation.Insert(
                    new LogMessage { Level = level, Message = message });
            table.Execute(insertOperation);
        }
    }
}