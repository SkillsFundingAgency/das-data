﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Infrastructure.Services;
using SFA.DAS.Provider.Events.Api.Client;
using SFA.DAS.Provider.Events.Api.Types;

namespace SFA.DAS.Data.Infrastructure.UnitTests.Services.ProviderEventsServiceTests
{
    [TestFixture]
    public class WhenIGetUnprocessedPeriodEnds
    {
        private Mock<IPaymentsEventsApiClient> _eventsApi;
        private Mock<IEventRepository> _eventsRepository;
        private ProviderEventsService _service;

        [SetUp]
        public void Arrange()
        {
            _eventsApi = new Mock<IPaymentsEventsApiClient>();
            _eventsRepository = new Mock<IEventRepository>();

            _service = new ProviderEventsService(_eventsApi.Object, _eventsRepository.Object);
        }

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

            _eventsRepository.Setup(x => x.GetLastProcessedEventId<string>("PeriodEnd")).ReturnsAsync(lastEventId);
            _eventsApi.Setup(x => x.GetPeriodEnds()).ReturnsAsync(returnedPeriods.ToArray());

            var response = await _service.GetUnprocessedPeriodEnds();

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

            _eventsRepository.Setup(x => x.GetLastProcessedEventId<string>("PeriodEnd")).ReturnsAsync(lastEventId);
            _eventsApi.Setup(x => x.GetPeriodEnds()).ReturnsAsync(returnedPeriods.ToArray());

            var response = await _service.GetUnprocessedPeriodEnds();

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

            _eventsRepository.Setup(x => x.GetLastProcessedEventId<string>("PeriodEnd")).ReturnsAsync(lastEventId);
            _eventsApi.Setup(x => x.GetPeriodEnds()).ReturnsAsync(returnedPeriods.ToArray());

            var response = await _service.GetUnprocessedPeriodEnds();

            response.ShouldBeEquivalentTo(returnedPeriods);
        }
    }
}