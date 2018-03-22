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
    public class PaymentStatisticsTests : FunctionEventTestBase
    {
        [SetUp]
        public async Task Setup()
        {
            DataTypes = "'ProviderTotalPayments'";

            var client = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString).CreateCloudQueueClient();
            var queue = client.GetQueueReference(QueueNames.ProviderQueueName);
            await queue.CreateIfNotExistsAsync();
            await queue.ClearAsync();

            var message = new CommitmentProcessingCompletedMessage
            {
                ProcessingCompletedAt = DateTime.UtcNow
            };

            await queue.AddMessageAsync(new CloudQueueMessage(JsonConvert.SerializeObject(message)));
        }

        [Test]
        public async Task WhenTheQueueFunctionIsRunThenTheStatisticsAreSavedToTheDatabase()
        {
            // sleep for a few seconds to allow the function to kick in once it detects a queue message
            Thread.Sleep(2000);

            var actual = await WithConnection(async c => await c.ExecuteScalarAsync<int>(
                sql: SqlVerificationScript(),
                commandType: CommandType.Text));

            Assert.AreEqual(1, actual);
        }
    }
}
