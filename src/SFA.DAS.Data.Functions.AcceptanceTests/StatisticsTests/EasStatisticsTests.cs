using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using NUnit.Framework;

namespace SFA.DAS.Data.Functions.AcceptanceTests.StatisticsTests
{
    [TestFixture]
    public class EasStatisticsTests : FunctionEventTestBase
    {
        [SetUp]
        public async Task Setup()
        {
            var client = CloudStorageAccount
                .Parse(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString)
                .CreateCloudQueueClient();
            var queue = client.GetQueueReference(QueueNames.CommitmentsQueueName);
            await queue.DeleteIfExistsAsync();
        }

        [Test]
        public async Task WhenTheTimerFunctionIsRunThenTheStatisticsAreSavedToTheDatabase()
        {
            // sleep for a few seconds to allow the timer function to kick in
            Thread.Sleep(5000);

            DataTypes = "'TotalPayments', 'TotalAccounts', 'TotalAgreements', 'TotalLegalEntities', 'TotalPAYESchemes'";

            var actual = await WithConnection(async c => await c.ExecuteScalarAsync<int>(
                sql: SqlVerificationScript(),
                commandType: CommandType.Text));
            
            Console.WriteLine(SqlVerificationScript());

            Assert.AreEqual(5, actual);
        }
    }
}
