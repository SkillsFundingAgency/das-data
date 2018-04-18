using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using NUnit.Framework;

namespace SFA.DAS.Data.Functions.AcceptanceTests.StatisticsTests
{
    [TestFixture]
    public class CommitmentStatisticsTests : FunctionEventTestBase
    {
        private const string DataTypes = "TotalCohorts, TotalApprenticeships, ActiveApprenticeships";

        [SetUp]
        public async Task Setup()
        {
            var client = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString).CreateCloudQueueClient();
            var queue = client.GetQueueReference(QueueNames.CommitmentsQueueName);
            await queue.CreateIfNotExistsAsync();
            await queue.ClearAsync();

            var message = new EasProcessingCompletedMessage
            {
                ProcessingCompletedAt = DateTime.UtcNow
            };

            await queue.AddMessageAsync(new CloudQueueMessage(JsonConvert.SerializeObject(message)));
        }

        [Test]
        public async Task WhenTheQueueFunctionIsRunThenTheStatisticsAreSavedToTheDatabase()
        {
            Thread.Sleep(3000);

            var actual = await WithConnection(async c => await c.ExecuteScalarAsync<int>(
                sql:
            $"SELECT count('Id') FROM [Data_Load].[DAS_ConsistencyCheck] WHERE " +
                $"CheckedDateTime >= '{TestOperationStartedAt.ToUniversalTime():yyyy-MM-ddTHH:mm:ss}'" +
                $" AND DataType IN ('{DataTypes}');",
                commandType: CommandType.Text));
            Console.WriteLine($"{TestOperationStartedAt.ToUniversalTime():yyyy-MM-ddTHH:mm:ss}");
            Assert.AreEqual(3, actual);
        }
    }
}
