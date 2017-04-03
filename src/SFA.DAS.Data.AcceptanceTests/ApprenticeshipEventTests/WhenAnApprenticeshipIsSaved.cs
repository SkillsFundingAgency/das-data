using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.Events.Api.Types;

namespace SFA.DAS.Data.AcceptanceTests.ApprenticeshipEventTests
{
    [TestFixture]
    public class WhenAnApprenticeshipIsSaved : ApprenticeshipEventTestsBase
    {
        [Test]
        public void ThenTheApprenticeshipDetailsAreStored()
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
            var lastProcessedEventId = await EventTestsRepository.GetLastProcessedEventId("ApprenticeshipEventView");
            if (lastProcessedEventId != 4)
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

        private void ConfigureEventsApi()
        {
            var events = new List<ApprenticeshipEventView>
            {
                new ApprenticeshipEventView
                {
                    CreatedOn = DateTime.Now.AddDays(-2),
                    Id = 3,
                    Event = "ApprenticeshipCreated",
                    TrainingStartDate = DateTime.Now.AddDays(1),
                    TrainingEndDate = DateTime.Now.AddYears(2),
                    AgreementStatus = AgreementStatus.NotAgreed,
                    PaymentStatus = PaymentStatus.Completed
                },
                new ApprenticeshipEventView
                {
                    CreatedOn = DateTime.Now.AddDays(-1),
                    Id = 4,
                    Event = "ApprenticeshipUpdated",
                    TrainingStartDate = DateTime.Now.AddDays(2),
                    TrainingEndDate = DateTime.Now.AddYears(3),
                    AgreementStatus = AgreementStatus.EmployerAgreed,
                    PaymentStatus = PaymentStatus.Active
                }
            };

            EventsApi.SetupGet("api/events/apprenticeships?fromEventId=3&pageSize=1000&pageNumber=1", events);
        }
    }
}
