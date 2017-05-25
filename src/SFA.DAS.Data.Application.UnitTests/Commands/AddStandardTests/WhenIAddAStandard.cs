using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apprenticeships.Api.Types;
using SFA.DAS.Data.Application.Commands.AddStandard;
using SFA.DAS.Data.Application.Interfaces.Gateways;
using SFA.DAS.Data.Application.Interfaces.Repositories;

namespace SFA.DAS.Data.Application.UnitTests.Commands.AddStandardTests
{
    [TestFixture]
    public class WhenIAddAStandard
    {
        private AddStandardCommandHandler _commandHandler;
        private Mock<IStandardRepository> _standardRepository;
        private Mock<IStandardGateway> _standardGateway;
        
        [SetUp]
        public void Arrange()
        {
            _standardRepository = new Mock<IStandardRepository>();
            _standardGateway = new Mock<IStandardGateway>();
            
            _commandHandler = new AddStandardCommandHandler(_standardRepository.Object, _standardGateway.Object);
        }

        [Test]
        public async Task ThenTheStandardDataIsRetrievedAndSaved()
        {
            var expectedStandard = new Standard();
            var standardId = "34589345";
            
            _standardGateway.Setup(x => x.GetStandard(standardId)).ReturnsAsync(expectedStandard);

            await _commandHandler.Handle(new AddStandardCommand { StandardId = standardId });

            _standardRepository.Verify(x => x.SaveStandard(expectedStandard), Times.Once);
        }
    }
}
