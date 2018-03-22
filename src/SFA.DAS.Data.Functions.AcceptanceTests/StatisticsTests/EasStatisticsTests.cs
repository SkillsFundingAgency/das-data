using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using NUnit.Framework;

namespace SFA.DAS.Data.Functions.AcceptanceTests.StatisticsTests
{
    [TestFixture]
    public class EasStatisticsTests : FunctionEventTestBase
    {
        [Test]
        public async Task WhenTheTimerFunctionIsRunThenTheStatisticsAreSavedToTheDatabase()
        {
            // sleep for a few seconds to allow the function to kick in once it detects a queue message
            Thread.Sleep(2000);

            DataTypes = "'TotalPayments', 'TotalAccounts', 'TotalAgreements', 'TotalLegalEntities', 'TotalPAYESchemes'";

            var actual = await WithConnection(async c => await c.ExecuteScalarAsync<int>(
                sql: SqlVerificationScript(),
                commandType: CommandType.Text));
            
            Assert.AreEqual(5, actual);
        }
    }
}
