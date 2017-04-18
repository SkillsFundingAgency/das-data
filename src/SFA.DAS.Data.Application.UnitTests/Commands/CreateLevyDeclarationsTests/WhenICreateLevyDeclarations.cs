using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Commands.CreateLevyDeclarations;
using SFA.DAS.Data.Application.Interfaces.Gateways;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.EAS.Account.Api.Types;

namespace SFA.DAS.Data.Application.UnitTests.Commands.CreateLevyDeclarationsTests
{
    [TestFixture]
    public class WhenICreateLevyDeclarations
    {
        private CreateLevyDeclarationsCommandHandler _commandHandler;
        private Mock<ILevyDeclarationRepository> _levyDeclarationRepository;
        private Mock<IAccountGateway> _accountGateway;
        
        [SetUp]
        public void Arrange()
        {
            _levyDeclarationRepository = new Mock<ILevyDeclarationRepository>();
            _accountGateway = new Mock<IAccountGateway>();
            
            _commandHandler = new CreateLevyDeclarationsCommandHandler(_levyDeclarationRepository.Object, _accountGateway.Object);
        }

        [Test]
        public async Task ThenTheLevyDeclarationsAreRetrievedAndSaved()
        {
            var levyDeclarations = new List<LevyDeclarationViewModel>
            {
                new LevyDeclarationViewModel(),
                new LevyDeclarationViewModel(),
                new LevyDeclarationViewModel()
            };
            var levyDeclarationHref = $"/api/accounts/ABC123/levy/";
            
            _accountGateway.Setup(x => x.GetLevyDeclarations(levyDeclarationHref)).ReturnsAsync(levyDeclarations);

            await _commandHandler.Handle(new CreateLevyDeclarationsCommand { LevyDeclarationsHref = levyDeclarationHref });

            _levyDeclarationRepository.Verify(x => x.SaveLevyDeclaration(It.IsAny<LevyDeclarationViewModel>()), Times.Exactly(3));
            _levyDeclarationRepository.Verify(x => x.SaveLevyDeclaration(levyDeclarations[0]), Times.Once);
            _levyDeclarationRepository.Verify(x => x.SaveLevyDeclaration(levyDeclarations[1]), Times.Once);
            _levyDeclarationRepository.Verify(x => x.SaveLevyDeclaration(levyDeclarations[2]), Times.Once);
        }
    }
}
