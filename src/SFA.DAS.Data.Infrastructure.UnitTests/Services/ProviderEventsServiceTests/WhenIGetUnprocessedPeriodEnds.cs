using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Provider.Events.Api.Types;

namespace SFA.DAS.Data.Infrastructure.UnitTests.Services.ProviderEventsServiceTests
{
    [TestFixture]
    public class WhenIGetUnprocessedPeriodEnds : ProviderEventsServiceTestsBase
    {
        [Test]
        public async Task AndThereAreUnprocessedPeriodsThenThePeriodsAreReturned()
        {
            var lastEventId = "123";
            var returnedPeriods = new List<PeriodEnd>
            {
                new PeriodEnd { Id = "sdfkujf" },
                new PeriodEnd { Id = lastEventId },
                new PeriodEnd { Id = "dlfkbmbg" },
                new PeriodEnd { Id = "cdvflkbc" }
            };

            var expectedPeriods = returnedPeriods.SkipWhile(x => x.Id != lastEventId.ToString()).Skip(1);

            EventsRepository.Setup(x => x.GetLastProcessedEventId<string>("PeriodEnd")).ReturnsAsync(lastEventId);
            EventsApi.Setup(x => x.GetPeriodEnds()).ReturnsAsync(returnedPeriods.ToArray());

            var response = await Service.GetUnprocessedPeriodEnds();

            response.ShouldBeEquivalentTo(expectedPeriods);
        }

        [Test]
        public async Task AndThereAreNoUnprocessedPeriodsThenNoPeriodsAreReturned()
        {
            var lastEventId = "123";
            var returnedPeriods = new List<PeriodEnd>
            {
                new PeriodEnd { Id = "clbmcvb" },
                new PeriodEnd { Id = lastEventId.ToString() }
            };

            EventsRepository.Setup(x => x.GetLastProcessedEventId<string>("PeriodEnd")).ReturnsAsync(lastEventId);
            EventsApi.Setup(x => x.GetPeriodEnds()).ReturnsAsync(returnedPeriods.ToArray());

            var response = await Service.GetUnprocessedPeriodEnds();

            response.Should().BeEmpty();
        }

        [Test]
        public async Task AndNoPeriodsHaveBeenPreviouslyProcessedThenAllPeriodsAreReturned()
        {
            var lastEventId = "";
            var returnedPeriods = new List<PeriodEnd>
            {
                new PeriodEnd { Id = "509mkfdb" },
                new PeriodEnd { Id = "!cvlkbjgnvb" }
            };

            EventsRepository.Setup(x => x.GetLastProcessedEventId<string>("PeriodEnd")).ReturnsAsync(lastEventId);
            EventsApi.Setup(x => x.GetPeriodEnds()).ReturnsAsync(returnedPeriods.ToArray());

            var response = await Service.GetUnprocessedPeriodEnds();

            response.ShouldBeEquivalentTo(returnedPeriods);
        }
    }
}
