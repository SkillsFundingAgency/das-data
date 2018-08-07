using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.Data.Domain.Models.Statistics.Payments;

namespace SFA.DAS.Data.AcceptanceTests.Functions.Statistics.Payments
{
    public class WhenIGetPaymentsStatistics : PaymentsTestBase
    {

        [Test]
        public async Task ThenSavePaymentsStatistics()
        {
            await SetupDatabase();

            var paymentsExternalModel = new PaymentExternalModel()
            {
                ProviderTotalPayments = 1
            };
            PaymentsStatisticsHandlerMock.Setup(x => x.Handle()).Returns(Task.FromResult(paymentsExternalModel));

            var traceLogger = new TraceWriterStub(TraceLevel.Verbose);

            await DAS.Data.Functions.Statistics.GetPaymentsStatisticsFunction.Run(null, traceLogger, Log, StatisticsService);

            var databaseAsExpected = TestHelper.ConditionMet(IsDatabaseInExpectedState, TimeSpan.FromSeconds(60));

            Assert.IsTrue(databaseAsExpected);
        }

        private async Task<bool> IsDatabaseInExpectedState()
        {
            var paymentsStatisticsCount = await StatisticsTestsRepository.GetNumberOfProviderTotalPayments();

            return paymentsStatisticsCount == 1;
        }
    }
}
