using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Castle.DynamicProxy.Generators.Emitters;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using NUnit.Framework;
using SFA.DAS.Data.Functions.AcceptanceTests.Infrastructure;
using StructureMap;

namespace SFA.DAS.Data.Functions.AcceptanceTests
{
    public abstract class FunctionEventTestBase
    {
        protected JobHost JobHostInstance;
        protected CancellationToken TestCancellationToken;
        protected DateTime TestOperationStartedAt;
        protected static List<Process> Processes = new List<Process>();
        protected string DataTypes;

        protected string SqlVerificationScript()
        {
            return "SELECT count('Id') FROM [Data_Load].[DAS_ConsistencyCheck] WHERE " +
                $"CheckedDateTime >= '{TestOperationStartedAt.ToUniversalTime():yyyy-MM-ddTHH:mm:ss.fff}'" +
                $" AND DataType IN ({DataTypes})";
        }

        protected static async Task<CloudQueue> CreateCloudQueueIfNotExists(string queueName)
        {
            var client = CloudStorageAccount
                .Parse(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString)
                .CreateCloudQueueClient();
            var queue = client.GetQueueReference(queueName);
            await queue.CreateIfNotExistsAsync();
            await queue.ClearAsync();
            return queue;
        }

        [OneTimeSetUp]
        public async Task ClassSetup()
        {
            var process = new Process
            {
                StartInfo =
                {
                    FileName = @"C:\Program Files (x86)\Microsoft SDKs\Azure\Storage Emulator\AzureStorageEmulator.exe",
                    Arguments = $"start"
                }
            };
            process.Start();
            Processes.Add(process);
            //give the emulator time to spin up
            Thread.Sleep(300);
            var config = new JobHostConfiguration();
            config.UseTimers();
            config.Tracing.ConsoleLevel = TraceLevel.Verbose;
            config.UseDependencyInjection();
            JobHostInstance = new JobHost(config);
            TestCancellationToken = new CancellationToken();
            TestOperationStartedAt = DateTime.UtcNow;

            Console.WriteLine(TestOperationStartedAt);

            await JobHostInstance.StartAsync(TestCancellationToken);
        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
            await JobHostInstance.StopAsync();

            Processes?.ForEach(process =>
            {
                if (!process.HasExited)
                {
                    process.Kill();
                }
            });
            Processes?.Clear();
        }

        protected async Task<T> WithConnection<T>(Func<IDbConnection, Task<T>> getData)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString))
            {
                await connection.OpenAsync();
                return await getData(connection);
            }
        }
    }

}
