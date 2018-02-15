using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Commitments.Api.Types;
using SFA.DAS.Data.Application.Commands.CreateRelationship;
using SFA.DAS.Data.Application.Interfaces.Repositories;

namespace SFA.DAS.Data.Application.UnitTests.Commands.CreateRelationshipTests
{
    [TestFixture]
    public class WhenICreateARelationship
    {
        private CreateRelationshipCommandHandler _commandHandler;
        private Mock<IRelationshipRepository> _relationshipRepository;

        [SetUp]
        public void Arrange()
        {
            _relationshipRepository = new Mock<IRelationshipRepository>();

            _commandHandler = new CreateRelationshipCommandHandler(_relationshipRepository.Object);
        }

        [Test]
        public async Task ThenTheRelationshipIsSaved()
        {
            var relationship = new Relationship()
            {
                EmployerAccountId = 1,
                LegalEntityId = "2",
                LegalEntityAddress = "An Address",
                LegalEntityName = "A name",
                LegalEntityOrganisationType = Common.Domain.Types.OrganisationType.CompaniesHouse,
                ProviderId = 3,
                ProviderName = "Provider name",
                Verified = false
            };

            await _commandHandler.Handle(new CreateRelationshipCommand() {Relationship = relationship});

            _relationshipRepository.Verify(x=>x.CreateRelationship(relationship), Times.Once);
        }
    }
}
