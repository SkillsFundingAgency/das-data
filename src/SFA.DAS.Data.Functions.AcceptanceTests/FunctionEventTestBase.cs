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
using Microsoft.Azure.WebJobs;
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

        [OneTimeSetUp]
        public async Task ClassSetup()
        {
            var config = new JobHostConfiguration();
            config.UseTimers();
            config.Tracing.ConsoleLevel = TraceLevel.Verbose;
            config.UseDependencyInjection();
            JobHostInstance = new JobHost(config);
            TestCancellationToken = new CancellationToken();
            TestOperationStartedAt = DateTime.UtcNow;

            await JobHostInstance.StartAsync(TestCancellationToken);
        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
            await JobHostInstance.StopAsync();
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
