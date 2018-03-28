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
using SFA.DAS.Data.Application.Commands.Statistics;

namespace SFA.DAS.Data.Functions.AcceptanceTests.StatisticsTests
{
    [TestFixture]
    public class CommitmentStatisticsTests : FunctionEventTestBase
    {
        private CloudQueue _queue;

        [SetUp]
        public async Task Setup()
        {
            DataTypes = "'TotalCohorts', 'TotalApprenticeships', 'ActiveApprenticeships'";

            var client = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString).CreateCloudQueueClient();
            _queue = client.GetQueueReference(QueueNames.CommitmentsQueueName);
            await _queue.CreateIfNotExistsAsync();
            await _queue.ClearAsync();

            var message = new EasProcessingCompletedMessage
            {
                ProcessingCompletedAt = DateTime.UtcNow
            };

            await _queue.AddMessageAsync(new CloudQueueMessage(JsonConvert.SerializeObject(message)));
        }

        [Test]
        public async Task WhenTheQueueFunctionIsRunThenTheStatisticsAreSavedToTheDatabase()
        {
            // sleep for a few seconds to allow the function to kick in once it detects a queue message
            Thread.Sleep(1000);

            var actual = await WithConnection(async c => await c.ExecuteScalarAsync<int>(
                sql: SqlVerificationScript(),
                commandType: CommandType.Text));
            
            Console.WriteLine(SqlVerificationScript());

            Assert.IsTrue(actual >= 3);
        }
    }
}
