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
        private const string DataTypes = "TotalPayments, TotalAccounts, TotalAgreements, TotalLegalEntities, TotalPAYESchemes";

        [Test]
        public async Task WhenTheTimerFunctionIsRunThenTheStatisticsAreSavedToTheDatabase()
        {
            var actual = await WithConnection(async c => await c.ExecuteScalarAsync<int>(
                sql:
                $"SELECT count('Id') FROM [Data_Load].[DAS_ConsistencyCheck] WHERE " +
                $"CheckedDateTime >= '{TestOperationStartedAt.ToUniversalTime():yyyy-MM-ddTHH:mm:ss}'" +
                $" AND DataType IN ('{DataTypes}');",
                commandType: CommandType.Text));
            Console.WriteLine($"{TestOperationStartedAt.ToUniversalTime():yyyy-MM-ddTHH:mm:ss}");
            Assert.AreEqual(5, actual);
        }
    }
}
