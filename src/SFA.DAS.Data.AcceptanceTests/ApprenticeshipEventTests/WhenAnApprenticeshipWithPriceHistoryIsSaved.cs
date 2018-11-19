using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.Data.AcceptanceTests.Data.DTOs;
using SFA.DAS.Events.Api.Types;

namespace SFA.DAS.Data.AcceptanceTests.ApprenticeshipEventTests
{
    [TestFixture]
    public class WhenAnApprenticeshipWithPriceHistoryIsSaved : ApprenticeshipEventTestsBase
    {
        protected override string EventName => "";

        [Test]
        public async Task ThenTheApprenticeshipPriceHistoryDetailsAreStored()
        {
            var expectedApprenticeshipEvents = ConfigureEventsApi();

            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            Task.Run(() => WorkerRole.Run(), cancellationToken);

            var databaseAsExpected = TestHelper.ConditionMet(IsDatabaseInExpectedState, TimeSpan.FromSeconds(60));

            cancellationTokenSource.Cancel();
            Assert.IsTrue(databaseAsExpected);

            await ArePriceHistoryFieldsStoredCorrectly(expectedApprenticeshipEvents);
        }

        private async Task<bool> IsDatabaseInExpectedState()
        {
            var lastProcessedEventId = await EventTestsRepository.GetLastProcessedEventId<long>("ApprenticeshipEventView");
            if (lastProcessedEventId != 6)
            {
                return false;
            }

            var numberOfRegistrations = await EventTestsRepository.GetNumberOfApprenticeships();
            if (numberOfRegistrations != 2)
            {
                return false;
            }

            return true;
        }

        private async Task ArePriceHistoryFieldsStoredCorrectly(IList<ApprenticeshipEventView> expectedApprenticeshipEvents)
        {
            var apprenticeships = (await EventTestsRepository.GetApprenticeships()).OrderBy(c => c.Id).ToList();

            var expectedPriceHistory = expectedApprenticeshipEvents[0].PriceHistory.First();
            var actualApprenticeship = apprenticeships.Single(c => c.CommitmentID == 5);
            await AssertPriceHistoryDetailsMatch(
                expectedApprenticeshipEvents.First().PriceHistory.First(),
                apprenticeships.Single(c => c.CommitmentID == 5));

            await AssertPriceHistoryDetailsMatch(
                expectedApprenticeshipEvents.Skip(1).First().PriceHistory.First(), 
                apprenticeships.Single(c => c.CommitmentID == 6));
        }

        private async Task AssertPriceHistoryDetailsMatch(PriceHistory expectedPriceHistory, CommitmentsRecord actualApprenticeship)
        {
            Assert.AreEqual(expectedPriceHistory.EffectiveFrom, actualApprenticeship.EffectiveFromDate);
            Assert.AreEqual(expectedPriceHistory.EffectiveTo, actualApprenticeship.EffectiveToDate);
            Assert.AreEqual(expectedPriceHistory.TotalCost, actualApprenticeship.PriceHistoryTotalCost);
        }

        private IList<ApprenticeshipEventView> ConfigureEventsApi()
        {
            var events = new List<ApprenticeshipEventView>
            {
                new ApprenticeshipEventView
                {
                    CreatedOn = DateTime.Now.AddDays(-2),
                    Id = 5,
                    Event = "ApprenticeshipCreated",
                    TrainingStartDate = DateTime.Now.AddDays(1),
                    TrainingEndDate = DateTime.Now.AddYears(2),
                    AgreementStatus = AgreementStatus.NotAgreed,
                    PaymentStatus = PaymentStatus.Completed,
                    LegalEntityId = "LEID",
                    LegalEntityName = "LEName",
                    LegalEntityOrganisationType = "LEOrgType",
                    DateOfBirth = DateTime.Now.AddYears(-18),
                    PriceHistory = new List<PriceHistory>
                    {
                        new PriceHistory
                        {
                            EffectiveFrom = new DateTime(2018, 11, 01),
                            EffectiveTo = null,
                            TotalCost = 100
                        }
                    }
                },
                new ApprenticeshipEventView
                {
                    CreatedOn = DateTime.Now.AddDays(-1),
                    Id = 6,
                    Event = "ApprenticeshipUpdated",
                    AgreementStatus = AgreementStatus.EmployerAgreed,
                    PaymentStatus = PaymentStatus.Active,
                    TransferApprovalActionedOn = DateTime.Today,
                    TransferApprovalStatus = TransferApprovalStatus.Pending,
                    TransferSenderName = "ignore me",
                    TransferSenderId = 38,
                    PriceHistory = new List<PriceHistory>
                    {
                        new PriceHistory
                        {
                            EffectiveFrom = new DateTime(2018, 11, 05, 10, 01, 01),
                            EffectiveTo = null,
                            TotalCost = 100
                        },
                    new PriceHistory
                        {
                            EffectiveFrom = new DateTime(2018, 11, 01),
                            EffectiveTo = new DateTime(2018, 11, 05, 10, 01, 00),
                            TotalCost = 100
                        }
                    }
                }
            };

            EventsApi.SetupGet("api/events/apprenticeships?fromEventId=3&pageSize=10000&pageNumber=1", events);

            return events;
        }
    }
}
