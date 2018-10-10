using System;
using System.Configuration;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Dapper;
using NUnit.Framework;
using SFA.DAS.Data.Functions.AcceptanceTests.Infrastructure;

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
        }

        [Test]
        public async Task WhenTheTimerFunctionIsRunThenTheStatisticsAreSavedToTheDatabase()
        {
            var maxRetries = int.Parse(ConfigurationManager.AppSettings["MaximumRetries"] ?? "3");
            var retryCount = 0;
            var expected = 5;
            int actual;

            do
            {
                // sleep for a few seconds to allow the timer function to kick in
                Thread.Sleep(2500);

                DataTypes = "'TotalPayments', 'TotalAccounts', 'TotalAgreements', 'TotalLegalEntities', 'TotalPAYESchemes'";

                actual = await WithConnection(async c => await c.ExecuteScalarAsync<int>(
                    sql: SqlVerificationScript(),
                    commandType: CommandType.Text));
            } while (actual != expected && retryCount++ < maxRetries);

            Console.WriteLine($"After {retryCount} retries result is {actual} and expected is {expected}");
            Console.WriteLine(SqlVerificationScript());

            Assert.AreEqual(expected, actual);
        }
    }
}
