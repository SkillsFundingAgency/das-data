using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Commands.CreateRegistration;
using SFA.DAS.Data.Application.Interfaces.Gateways;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.EAS.Account.Api.Client.Dtos;

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
            var expectedRegistration1 = new AccountInformationViewModel();
            var expectedRegistration2 = new AccountInformationViewModel();
            var expectedRegistrations = new List<AccountInformationViewModel> {expectedRegistration1, expectedRegistration2};
            var dasAccountId = "23454";

            _registrationGateway.Setup(x => x.GetRegistration(dasAccountId)).ReturnsAsync(expectedRegistrations);

            await _commandHandler.Handle(new CreateRegistrationCommand {DasAccountId = dasAccountId});

            _registrationRepository.Verify(x => x.SaveRegistration(expectedRegistration1), Times.Once);
            _registrationRepository.Verify(x => x.SaveRegistration(expectedRegistration2), Times.Once);
        }
    }
}
