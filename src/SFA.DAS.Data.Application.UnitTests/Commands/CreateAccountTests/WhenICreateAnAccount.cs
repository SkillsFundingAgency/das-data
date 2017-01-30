using System.Threading.Tasks;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Commands.CreateLegalEntity;
using SFA.DAS.Data.Application.Commands.CreatePayeScheme;
using SFA.DAS.Data.Application.Commands.CreateRegistration;
using SFA.DAS.Data.Application.Interfaces.Gateways;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Tests.Builders;

namespace SFA.DAS.Data.Application.UnitTests.Commands.CreateAccountTests
{
    [TestFixture]
    public class WhenICreateAnAccount
    {
        private CreateAccountCommandHandler _commandHandler;
        private Mock<IAccountRepository> _registrationRepository;
        private Mock<IAccountGateway> _registrationGateway;
        private Mock<IMediator> _mediator;

        [SetUp]
        public void Arrange()
        {
            _registrationRepository = new Mock<IAccountRepository>();
            _registrationGateway = new Mock<IAccountGateway>();
            _mediator = new Mock<IMediator>();

            _commandHandler = new CreateAccountCommandHandler(_registrationRepository.Object, _registrationGateway.Object, _mediator.Object);
        }

        [Test]
        public async Task ThenTheAccountDataIsRetrievedAndSaved()
        {
            var expectedAccount = new AccountDetailViewModelBuilder().Build();
            var accountHref = $"/api/accounts/{expectedAccount.DasAccountId}";
            
            _registrationGateway.Setup(x => x.GetAccount(accountHref)).ReturnsAsync(expectedAccount);

            await _commandHandler.Handle(new CreateAccountCommand { AccountHref = accountHref });

            _registrationRepository.Verify(x => x.SaveAccount(expectedAccount), Times.Once);
            _mediator.Verify(x => x.PublishAsync(It.Is<CreateLegalEntityCommand>(c => c.LegalEntityHref == expectedAccount.LegalEntities[0].Href)), Times.Once);
            _mediator.Verify(x => x.PublishAsync(It.Is<CreatePayeSchemeCommand>(c => c.PayeSchemeHref == expectedAccount.PayeSchemes[0].Href)), Times.Once);
        }
    }
}
