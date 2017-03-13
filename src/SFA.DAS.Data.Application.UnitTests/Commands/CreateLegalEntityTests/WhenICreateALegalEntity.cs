using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Commands.CreateLegalEntity;
using SFA.DAS.Data.Application.Interfaces.Gateways;
using SFA.DAS.Data.Domain.Interfaces.Repositories;
using SFA.DAS.Data.Tests.Builders;

namespace SFA.DAS.Data.Application.UnitTests.Commands.CreateLegalEntityTests
{
    [TestFixture]
    public class WhenICreateALegalEntity
    {
        private CreateLegalEntityCommandHandler _commandHandler;
        private Mock<ILegalEntityRepository> _legalEntityRepository;
        private Mock<IAccountGateway> _accountGateway;
        
        [SetUp]
        public void Arrange()
        {
            _legalEntityRepository = new Mock<ILegalEntityRepository>();
            _accountGateway = new Mock<IAccountGateway>();
            
            _commandHandler = new CreateLegalEntityCommandHandler(_legalEntityRepository.Object, _accountGateway.Object);
        }

        [Test]
        public async Task ThenTheAccountDataIsRetrievedAndSaved()
        {
            var expectedLegalEntity = new LegalEntityViewModelBuilder().Build();
            var legalEntityHref = $"/api/accounts/{expectedLegalEntity.DasAccountId}/legalentities/{expectedLegalEntity.LegalEntityId}";
            
            _accountGateway.Setup(x => x.GetLegalEntity(legalEntityHref)).ReturnsAsync(expectedLegalEntity);

            await _commandHandler.Handle(new CreateLegalEntityCommand() { LegalEntityHref = legalEntityHref });

            _legalEntityRepository.Verify(x => x.SaveLegalEntity(expectedLegalEntity), Times.Once);
        }
    }
}
