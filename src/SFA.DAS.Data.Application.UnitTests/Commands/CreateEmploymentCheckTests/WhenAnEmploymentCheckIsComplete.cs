using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Commands.CreateEmploymentCheck;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.EmploymentCheck.Events;

namespace SFA.DAS.Data.Application.UnitTests.Commands.CreateEmploymentCheckTests
{
    [TestFixture]
    public class WhenAnEmploymentCheckIsCompleted
    {
        private CreateEmploymentCheckCommandHandler _commandHandler;
        private Mock<IEmploymentCheckRepository> _repository;

        [SetUp]
        public void Arrange()
        {
            _repository = new Mock<IEmploymentCheckRepository>();
            
            _commandHandler = new CreateEmploymentCheckCommandHandler(_repository.Object);
        }

        [Test]
        public async Task ThenTheEmploymentCheckIsSaved()
        {
            var expectedEmploymentCheck = new EmploymentCheckCompleteEvent();
            
            await _commandHandler.Handle(new CreateEmploymentCheckCommand { Event = expectedEmploymentCheck });

            _repository.Verify(x => x.SaveEmploymentCheck(expectedEmploymentCheck), Times.Once);
        }
    }
}
