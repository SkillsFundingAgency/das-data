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

            var queue = await CreateCloudQueueIfNotExists(QueueNames.ProviderQueueName);

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
            Thread.Sleep(5000);

            var actual = await WithConnection(async c => await c.ExecuteScalarAsync<int>(
                sql: SqlVerificationScript(),
                commandType: CommandType.Text));

            Console.WriteLine(SqlVerificationScript());

            Assert.AreEqual(1, actual);
        }
    }
}
