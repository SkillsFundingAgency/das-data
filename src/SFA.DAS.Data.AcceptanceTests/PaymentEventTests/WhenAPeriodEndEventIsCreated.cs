using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.Data.Tests.Builders;
using SFA.DAS.Provider.Events.Api.Types;

namespace SFA.DAS.Data.AcceptanceTests.PaymentEventTests
{
    [TestFixture]
    public class WhenAPeriodEndEventIsCreated : PaymentEventTestsBase
    {
        protected override string EventName => "";

        [Test]
        public void ThenThePaymentsAreStored()
        {
            ConfigureEventsApi();
            
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            Task.Run(() => WorkerRole.Run(), cancellationToken);

            var databaseAsExpected = TestHelper.ConditionMet(IsDatabaseInExpectedState, TimeSpan.FromSeconds(60));

            cancellationTokenSource.Cancel();
            Assert.IsTrue(databaseAsExpected);
        }

        private async Task<bool> IsDatabaseInExpectedState()
        {
            var lastProcessedEventId = await EventTestsRepository.GetLastProcessedEventId<string>("PeriodEnd");
            if (lastProcessedEventId != "PERIOD4")
            {
                return false;
            }

            var numberOfPayments = await EventTestsRepository.GetNumberOfPayments();
            if (numberOfPayments != 7)
            {
                return false;
            }

            return true;
        }

        private void ConfigureEventsApi()
        {
            ConfigurePeriodEnds();
            ConfigurePayments("PERIOD3", 3);
            ConfigurePayments("PERIOD4", 4);
        }

        private void ConfigurePeriodEnds()
        {
            var periodEnds = new[]
            {
                new PeriodEnd { Id = "PERIOD1" },
                new PeriodEnd { Id = "PERIOD2" },
                new PeriodEnd { Id = "PERIOD3" },
                new PeriodEnd { Id = "PERIOD4" }
            };

            EventsApi.SetupGet("api/periodends", periodEnds);
        }

        private void ConfigurePayments(string periodEnd, int numberOfPayments)
        {
            var payments = new List<Payment>();
            for (var i = 1; i <= numberOfPayments; i++)
            {
                payments.Add(new PaymentBuilder().WithId(periodEnd + i).WithPeriod(periodEnd).Build());
            }
            var paymentsResult = new PageOfResults<Payment> { Items = payments.ToArray(), PageNumber = 1, TotalNumberOfPages = 1 };
            EventsApi.SetupGet($"api/payments?page=1&periodId={periodEnd}&employerAccountId={null}&ukprn={null}", paymentsResult);
            EventsApi.SetupGet($"api/transfers?page=1&periodId={periodEnd}", new PageOfResults<Payment> {PageNumber = 0, TotalNumberOfPages = 0});
        }
    }
}
