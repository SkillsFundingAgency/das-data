using System;
using System.Configuration;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using NUnit.Framework;

namespace SFA.DAS.Data.Functions.AcceptanceTests.StatisticsTests
{
    [TestFixture]
    public class PaymentStatisticsTests : FunctionEventTestBase
    {
        [SetUp]
        public async Task Setup()
        {
            DataTypes = "'ProviderTotalPayments','ProviderTotalPaymentsWithRequestedPayment'";
        }

        [Test]
        public async Task WhenTheTimerFunctionIsRunThenTheStatisticsAreSavedToTheDatabase()
        {
            var maxRetries = int.Parse(ConfigurationManager.AppSettings["MaximumRetries"] ?? "3");
            var retryCount = 0;
            var expected = 2;
            int actual;

            do
            {
                // sleep for a few seconds to allow the function to kick in once it detects a queue message
                Thread.Sleep(2500);

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
