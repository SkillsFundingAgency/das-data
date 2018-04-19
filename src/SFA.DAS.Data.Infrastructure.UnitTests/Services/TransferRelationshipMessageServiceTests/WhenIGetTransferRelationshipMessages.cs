using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Domain.Models;
using SFA.DAS.Data.Infrastructure.Services;
using SFA.DAS.EmployerAccounts.Events.Messages;
using SFA.DAS.Events.Api.Client;
using SFA.DAS.Events.Api.Types;

namespace SFA.DAS.Data.Infrastructure.UnitTests.Services.TransferRelationshipMessageServiceTests
{
    [TestFixture]
    public class WhenIGetTransferRelationshipMessages
    {
        
        private Mock<ITransferRelationshipRepository> _transferRelationshipRepositoryMock;
        private TransferRelationshipMessageService _service;

        [SetUp]
        public void Arrange()
        {
            _transferRelationshipRepositoryMock = new Mock<ITransferRelationshipRepository>(MockBehavior.Strict);

            _service = new TransferRelationshipMessageService(_transferRelationshipRepositoryMock.Object);

        }

        [Test]
        public void AndIsATransferSentMessageThenSaveMessageDetails()
        {
            var sentMessage = new SentTransferConnectionInvitationEvent();
            _transferRelationshipRepositoryMock.Setup(s => s.SaveTransferRelationship(It.IsAny<TransferRelationship>())).Returns(Task.FromResult(true));

            _service.SaveSentMessage(sentMessage);

            _transferRelationshipRepositoryMock.Verify(v => v.SaveTransferRelationship(It.IsAny<TransferRelationship>()));
        }

        [Test]
        public void AndIsATransferApprovedMessageThenSaveMessageDetails()
        {
            var approvedMessage = new ApprovedTransferConnectionInvitationEvent();
            _transferRelationshipRepositoryMock.Setup(s => s.SaveTransferRelationship(It.IsAny<TransferRelationship>())).Returns(Task.FromResult(true));

            _service.SaveApprovedMessage(approvedMessage);

            _transferRelationshipRepositoryMock.Verify(v => v.SaveTransferRelationship(It.IsAny<TransferRelationship>()));
        }

        [Test]
        public void AndIsATransferRejectedMessageThenSaveMessageDetails()
        {
            var rejectedMessage = new RejectedTransferConnectionInvitationEvent();
            _transferRelationshipRepositoryMock.Setup(s => s.SaveTransferRelationship(It.IsAny<TransferRelationship>())).Returns(Task.FromResult(true));

            _service.SaveRejectedMessage(rejectedMessage);

            _transferRelationshipRepositoryMock.Verify(v => v.SaveTransferRelationship(It.IsAny<TransferRelationship>()));
        }
    }
}
