using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Commands.AddRoatpProvider;
using SFA.DAS.Data.Application.Interfaces.Gateways;
using SFA.DAS.Data.Application.Interfaces.Repositories;

namespace SFA.DAS.Data.Application.UnitTests.Commands.AddRoatpProviderTests
{
    [TestFixture]
    public class WhenIAddARoatpProvider
    {
        private AddRoatpProviderCommandHandler _commandHandler;
        private Mock<IRoatpRepository> _roatpRepository;
        private Mock<IRoatpGateway> _roatpGateway;
        
        [SetUp]
        public void Arrange()
        {
            _roatpRepository = new Mock<IRoatpRepository>();
            _roatpGateway = new Mock<IRoatpGateway>();
            
            _commandHandler = new AddRoatpProviderCommandHandler(_roatpRepository.Object, _roatpGateway.Object);
        }

        [Test]
        public async Task ThenTheProviderDataIsRetrievedAndSaved()
        {
            var expectedProvider = new Roatp.Api.Types.Provider();
            var providerId = 34589345;
            
            _roatpGateway.Setup(x => x.GetProvider(providerId)).ReturnsAsync(expectedProvider);

            await _commandHandler.Handle(new AddRoatpProviderCommand { ProviderId = providerId });

            _roatpRepository.Verify(x => x.SaveRoatpProvider(expectedProvider), Times.Once);
        }
    }
}
