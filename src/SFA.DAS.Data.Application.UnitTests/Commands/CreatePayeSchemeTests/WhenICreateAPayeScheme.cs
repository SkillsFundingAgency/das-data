using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Commands.CreateLegalEntity;
using SFA.DAS.Data.Application.Commands.CreatePayeScheme;
using SFA.DAS.Data.Application.Interfaces.Gateways;
using SFA.DAS.Data.Domain.Interfaces.Repositories;
using SFA.DAS.Data.Tests.Builders;

namespace SFA.DAS.Data.Application.UnitTests.Commands.CreatePayeSchemeTests
{
    [TestFixture]
    public class WhenICreateAPayeScheme
    {
        private CreatePayeSchemeCommandHandler _commandHandler;
        private Mock<IPayeSchemeRepository> _payeSchemeRepository;
        private Mock<IAccountGateway> _accountGateway;
        
        [SetUp]
        public void Arrange()
        {
            _payeSchemeRepository = new Mock<IPayeSchemeRepository>();
            _accountGateway = new Mock<IAccountGateway>();
            
            _commandHandler = new CreatePayeSchemeCommandHandler(_payeSchemeRepository.Object, _accountGateway.Object);
        }

        [Test]
        public async Task ThenThePayeSchemeIsRetrievedAndSaved()
        {
            var expectedPayeScheme = new PayeSchemeViewModelBuilder().Build();
            var payeSchemeHref = $"/api/accounts/{expectedPayeScheme.DasAccountId}/payeschemes/{expectedPayeScheme.Ref}";
            
            _accountGateway.Setup(x => x.GetPayeScheme(payeSchemeHref)).ReturnsAsync(expectedPayeScheme);

            await _commandHandler.Handle(new CreatePayeSchemeCommand { PayeSchemeHref = payeSchemeHref });

            _payeSchemeRepository.Verify(x => x.SavePayeScheme(expectedPayeScheme), Times.Once);
        }
    }
}
