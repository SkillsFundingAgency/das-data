using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.Data.AcceptanceTests.PaymentEventTests;
using SFA.DAS.Data.Tests.Builders;
using SFA.DAS.Provider.Events.Api.Types;

namespace SFA.DAS.Data.AcceptanceTests.TransferEventTests
{
    [TestFixture]
    public class WhenAPeriodEndEventIsCreated : PaymentEventTestsBase
    {
        protected override string EventName => "";

        [Test]
        public async Task ThenTheTransfersAreStored()
        {
            // arrange
            var expectedTransfers = ConfigureEventsApi().OrderBy(t => t.RequiredPaymentId).ToList();
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            // act
            Task.Run(() => WorkerRole.Run(), cancellationToken);
            var periodEndFinished = TestHelper.ConditionMet(PeriodEndProcessed, TimeSpan.FromSeconds(60));
            cancellationTokenSource.Cancel();

            // assert
            Assert.IsTrue(periodEndFinished);

            var actualTransfers = (await EventTestsRepository.GetTransfers()).OrderBy(t => t.RequiredPaymentId).ToList();

            Assert.AreEqual(expectedTransfers.Count, actualTransfers.Count);

            for (var i = 0; i < actualTransfers.Count; i++)
            {
                Assert.AreEqual(expectedTransfers[i].Amount, actualTransfers[i].Amount);
                Assert.AreEqual(expectedTransfers[i].SenderAccountId, actualTransfers[i].SenderAccountId);
                Assert.AreEqual(expectedTransfers[i].ReceiverAccountId, actualTransfers[i].ReceiverAccountId);
                Assert.AreEqual(expectedTransfers[i].RequiredPaymentId, actualTransfers[i].RequiredPaymentId);
                Assert.AreEqual(expectedTransfers[i].Type, actualTransfers[i].Type);
                Assert.AreEqual(expectedTransfers[i].CommitmentId, actualTransfers[i].CommitmentId);
                Assert.AreEqual(expectedTransfers[i].CollectionPeriodName, actualTransfers[i].CollectionPeriodName);
            }
        }

        [TestCase(false)]
        [TestCase(true)]
        public async Task AndThereAreALotOfTransfersThenTheyAreProcessedInPages(bool skipPayments)
        {
            // skipPayments will generate error on parallel processing of payments, this should not affect transfers processing

            // arrange
            var expectedTransfers = ConfigureEventsApi(3, skipPayments).OrderBy(t => t.RequiredPaymentId).ToList();
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            // act
            Task.Run(() => WorkerRole.Run(), cancellationToken);
            var periodEndFinished = TestHelper.ConditionMet(PeriodEndProcessed, TimeSpan.FromSeconds(60));
            cancellationTokenSource.Cancel();

            // assert
            Assert.IsTrue(periodEndFinished);

            var actualCount = await EventTestsRepository.GetNumberOfTransfers();
            Assert.AreEqual(expectedTransfers.Count, actualCount);
        }

        private async Task<bool> PeriodEndProcessed()
        {
            return await EventTestsRepository.GetLastProcessedEventId<string>("PeriodEnd-AccountTransfer") == "PERIOD4";
        }

        private List<AccountTransfer> ConfigureEventsApi(int pages = 1, bool skipPayments = false)
        {
            var transfers = new List<AccountTransfer>();
            ConfigurePeriodEnds();
            transfers.AddRange(ConfigureTransfers("PERIOD3", 3, pages, skipPayments));
            transfers.AddRange(ConfigureTransfers("PERIOD4", 4, pages, skipPayments));
            return transfers;
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

        private List<AccountTransfer> ConfigureTransfers(string periodEnd, int numberOfTransfers, int repeatPages = 1, bool skipPayments = false)
        {
            var allTransfers = new List<AccountTransfer>();

            for (var k = 1; k <= repeatPages; k++)
            {
                var transfers = new List<AccountTransfer>();
                for (var i = 1; i <= numberOfTransfers; i++)
                {
                    transfers.Add(new TransferBuilder().WithPeriod(periodEnd).Build());
                }

                var transfersResult = new PageOfResults<AccountTransfer>
                {
                    Items = transfers.ToArray(),
                    PageNumber = k,
                    TotalNumberOfPages = repeatPages
                };

                EventsApi.SetupGet($"api/transfers?page={k}&periodId={periodEnd}", transfersResult);
                allTransfers.AddRange(transfers);
            }

            if (!skipPayments)
                EventsApi.SetupGet($"api/payments?page=1&periodId={periodEnd}&employerAccountId={null}&ukprn={null}", new PageOfResults<Payment> {PageNumber = 0, TotalNumberOfPages = 0});

            return allTransfers;
        }
    }
}
