using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Handlers;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Provider.Events.Api.Client;
using SFA.DAS.Provider.Events.Api.Types;

namespace SFA.DAS.Data.Application.UnitTests.Handler
{
    [TestFixture]
    public class GivenIRequestPaymentStatistics
    {
        private PaymentsStatisticsHandler _paymentsEventsHandler;
        private Mock<IPaymentsEventsApiClient> _paymentsEventsApiClientMock;
        private Mock<ILog> _loggerMock;

        [SetUp]
        public void Arrange()
        {
            _paymentsEventsApiClientMock = new Mock<IPaymentsEventsApiClient>(MockBehavior.Strict);
            _loggerMock = new Mock<ILog>();

            _paymentsEventsApiClientMock.Setup(w => w.GetPaymentStatistics()).ReturnsAsync(
                new PaymentStatistics()
                {
                    TotalNumberOfPayments = 1000,
                    TotalNumberOfPaymentsWithRequiredPayment = 990
                }).Verifiable();

            _paymentsEventsHandler = new PaymentsStatisticsHandler(_loggerMock.Object, _paymentsEventsApiClientMock.Object);
        }

        [Test]
        public async Task ThenThePaymentsStaticsticsApiIsCalled()
        {
            var result = await _paymentsEventsHandler.Handle();

            _paymentsEventsApiClientMock.Verify();

        }

        [Test]
        public async Task AndThenThePaymentsExternalModelIsReturnedCorrectly()
        {
            var result = await _paymentsEventsHandler.Handle();

            result.ProviderTotalPayments.Should().Be(1000);
            result.ProviderTotalPaymentsWithRequestedPayment.Should().Be(990);
        }
    }
}
