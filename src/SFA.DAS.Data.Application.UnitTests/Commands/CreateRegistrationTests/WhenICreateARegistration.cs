using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Commands.CreateRegistration;
using SFA.DAS.Data.Application.Dtos;
using SFA.DAS.Data.Application.Interfaces.Gateways;
using SFA.DAS.Data.Application.Interfaces.Repositories;

namespace SFA.DAS.Data.Application.UnitTests.Commands.CreateRegistrationTests
{
    [TestFixture]
    public class WhenICreateARegistration
    {
        private CreateRegistrationCommandHandler _commandHandler;
        private Mock<IRegistrationRepository> _registrationRepository;
        private Mock<IRegistrationGateway> _registrationGateway;

        [SetUp]
        public void Arrange()
        {
            _registrationRepository = new Mock<IRegistrationRepository>();
            _registrationGateway = new Mock<IRegistrationGateway>();

            _commandHandler = new CreateRegistrationCommandHandler(_registrationRepository.Object, _registrationGateway.Object);
        }

        [Test]
        public async Task ThenTheRegistrationDataIsRetrievedAndSaved()
        {
            var expectedRegistration = new RegistrationViewModel();
            var organisationId = 23454;

            _registrationGateway.Setup(x => x.GetRegistration(organisationId)).ReturnsAsync(expectedRegistration);

            await _commandHandler.Handle(new CreateRegistrationCommand {OrganisationId = organisationId});

            _registrationRepository.Verify(x => x.SaveRegistration(expectedRegistration), Times.Once);
        }
    }
}
