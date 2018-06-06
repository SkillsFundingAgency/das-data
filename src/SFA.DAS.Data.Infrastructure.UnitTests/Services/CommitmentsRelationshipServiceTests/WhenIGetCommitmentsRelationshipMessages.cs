using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Commitments.Api.Types;
using SFA.DAS.Commitments.Events;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Infrastructure.Services;

namespace SFA.DAS.Data.Infrastructure.UnitTests.Services.CommitmentsRelationshipServiceTests
{
    [TestFixture]
    public class WhenIGetCommitmentsRelationshipMessages
    {
        private Mock<ICommitmentsRelationshipRepository> _commitmentsRelationshipRepositoryMock;
        private CommitmentsRelationshipService _service;

        [SetUp]
        public void Arrange()
        {
            _commitmentsRelationshipRepositoryMock = new Mock<ICommitmentsRelationshipRepository>(MockBehavior.Strict);

            _service = new CommitmentsRelationshipService(_commitmentsRelationshipRepositoryMock.Object);
        }

        [Test]
        public void AndIsACreatedRelationshipThenSaveCreatedRelationship()
        {
            var relationshipCreated = new RelationshipCreated();
            relationshipCreated.Relationship = new Relationship();;
            _commitmentsRelationshipRepositoryMock.Setup(s => s.CreateCommitmentsRelationship(It.IsAny<Relationship>())).Returns(Task.FromResult(true));

            _service.SaveCreatedRelationship(relationshipCreated);

            _commitmentsRelationshipRepositoryMock.Verify(x=>x.CreateCommitmentsRelationship(It.IsAny<Relationship>()));
        }

        [Test]
        public void AndIsAVerifiedRelationshipThenUpdateRelationship()
        {
            var relationshipVerified = new RelationshipVerified();
            _commitmentsRelationshipRepositoryMock
                .Setup(s => s.VerifyCommitmentsRelationship(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<string>(),
                    It.IsAny<bool?>())).Returns(Task.FromResult(true));

            _service.SaveVerifiedRelationship(relationshipVerified);

            _commitmentsRelationshipRepositoryMock.Verify(x =>
                x.VerifyCommitmentsRelationship(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<string>(), It.IsAny<bool?>()));
        }
    }
}
