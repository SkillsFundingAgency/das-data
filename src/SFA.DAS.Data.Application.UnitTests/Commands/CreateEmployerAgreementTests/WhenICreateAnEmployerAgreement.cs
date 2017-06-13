using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Commands.CreateEmployerAgreement;
using SFA.DAS.Data.Application.Interfaces.Gateways;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.EAS.Account.Api.Types;

namespace SFA.DAS.Data.Application.UnitTests.Commands.CreateEmployerAgreementTests
{
    [TestFixture]
    public class WhenICreateAnEmployerAgreement
    {
        private CreateEmployerAgreementCommandHandler _commandHandler;
        private Mock<IEmployerAgreementRepository> _employerAgreementRepository;
        private Mock<IAccountGateway> _accountGateway;
        
        [SetUp]
        public void Arrange()
        {
            _employerAgreementRepository = new Mock<IEmployerAgreementRepository>();
            _accountGateway = new Mock<IAccountGateway>();
            
            _commandHandler = new CreateEmployerAgreementCommandHandler(_employerAgreementRepository.Object, _accountGateway.Object);
        }

        [Test]
        public async Task ThenTheAgreementDataIsRetrievedAndSaved()
        {
            var expectedAgreement = new EmployerAgreementView();
            var employerAgreementHref = $"/api/accounts/ABC123/agreements/ZZZZ999";
            
            _accountGateway.Setup(x => x.GetEmployerAgreement(employerAgreementHref)).ReturnsAsync(expectedAgreement);

            await _commandHandler.Handle(new CreateEmployerAgreementCommand { AgreementHref = employerAgreementHref });

            _employerAgreementRepository.Verify(x => x.SaveEmployerAgreement(expectedAgreement), Times.Once);
        }
    }
}
