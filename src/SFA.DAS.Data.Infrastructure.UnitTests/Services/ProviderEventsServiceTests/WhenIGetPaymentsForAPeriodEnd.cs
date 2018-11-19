using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Provider.Events.Api.Types;

namespace SFA.DAS.Data.Infrastructure.UnitTests.Services.ProviderEventsServiceTests
{
    [TestFixture]
    public class WhenIGetPaymentsForAPeriodEnd : ProviderEventsServiceTestsBase
    {
        [Test]
        public async Task ThenThePaymentsAreReturned()
        {
            var period = "ABC123";
            var pageNumber = 3;

            var expectedResponse = new PageOfResults<Payment>();

            EventsApi.Setup(x => x.GetPayments(period, null, pageNumber, null)).ReturnsAsync(expectedResponse);

            var response = await Service.GetPayments(period, pageNumber);

            response.Should().BeSameAs(expectedResponse);
        }
    }
}
