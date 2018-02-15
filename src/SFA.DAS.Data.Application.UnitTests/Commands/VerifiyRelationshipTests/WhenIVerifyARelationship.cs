using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Data.Application.Commands.VerifyRelationship;
using SFA.DAS.Data.Application.Interfaces.Repositories;

namespace SFA.DAS.Data.Application.UnitTests.Commands.VerifiyRelationshipTests
{
    [TestFixture]
    public class WhenIVerifyARelationship
    {
        private VerifyRelationshipCommandHandler _commandHandler;
        private Mock<IRelationshipRepository> _relationshipRepository;

        [SetUp]
        public void Arrange()
        {
            _relationshipRepository = new Mock<IRelationshipRepository>();

            _commandHandler = new VerifyRelationshipCommandHandler(_relationshipRepository.Object);
        }

        [Test]
        public async Task ThenTheRelationshipIsUpdated()
        {
            long employerAccountId = 1;
            long providerId = 2;
            string legalEntityId = "3";

            await _commandHandler.Handle(new VerifyRelationshipCommand
            {
                EmployerAccountId = employerAccountId,
                LegalEntityId = legalEntityId,
                ProviderId = providerId,
                Verified = false
            });

            _relationshipRepository.Verify(x =>
                x.VerifyRelationship(providerId, employerAccountId, legalEntityId, false));
        }
    }
}
