using System.Threading.Tasks;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Commands.CreateRoatpProvider;
using SFA.DAS.Data.Application.Interfaces.Gateways;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Tests.Builders;
using SFA.DAS.Events.Api.Types;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.Application.UnitTests.Commands.CreateRoatpProviderTests
{
    [TestFixture]
    public class WhenICreateAProvider
    {
        private CreateProviderCommandHandler _commandHandler;
        private Mock<IProviderRepository> _providerRepository;
        private Mock<IRoatpGateway> _roatpGateway;
        private Mock<ILog> _logger;
        private Mock<IMediator> _mediator;

        [SetUp]
        public void Arrange()
        {
            _providerRepository = new Mock<IProviderRepository>();
            _roatpGateway = new Mock<IRoatpGateway>();
            _logger = new Mock<ILog>();
            _mediator = new Mock<IMediator>();

            _commandHandler = new CreateProviderCommandHandler(_providerRepository.Object, _roatpGateway.Object, _logger.Object);
        }

        [Test]
        public async Task ThenTheProviderIsSaved()
        {
            var expectedProvider = new ProviderBuilder().Build();
            var ukprn = $"/api/providers/{expectedProvider.Ukprn}";

            _roatpGateway.Setup(x => x.GetProvider(expectedProvider.Ukprn.ToString())).Returns(expectedProvider);

            var evt = new AgreementEvent()
            {
                ContractType = "ProviderAgreement",
                Event = "INITIATED",
                ProviderId = "12345678"
            };

            await _commandHandler.Handle(new CreateProviderCommand() { Event = evt });

            _providerRepository.Verify(x => x.SaveProvider(expectedProvider), Times.Once);
        }
    }
}
