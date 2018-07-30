using System.Threading.Tasks;
using SFA.DAS.Commitments.Api.Types;
using SFA.DAS.Data.AcceptanceTests.Data;
using SFA.DAS.Data.AcceptanceTests.Data.DTOs;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Infrastructure.Data;
using SFA.DAS.Data.Infrastructure.Services;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Data.AcceptanceTests.Functions.Commitments
{
    public class CommitmentsTestBase
    {
        public CommitmentsTestRepository CommitmentsTestRepository;
        public ILog Log;
        public ICommitmentsRelationshipService CommitmentsRelationshipService;

        public async Task SetupDatabase()
        {
            Log = new NLogLogger(typeof(CommitmentsRelationshipService), null);
            CommitmentsRelationshipService = new CommitmentsRelationshipService(new CommitmentsRelationshipRepository(DataAcceptanceTests.Config.DatabaseConnectionString));
            CommitmentsTestRepository = new CommitmentsTestRepository(DataAcceptanceTests.Config.DatabaseConnectionString);
            await CommitmentsTestRepository.DeleteCommitmentsRelationships();
        }

        public async Task InsertCommitmentsRelationship(Relationship relationship)
        {
            var relationshipsRecord = new CommitmentsRelationshipsRecord
            {
                Id=relationship.Id,
                ProviderId = relationship.ProviderId,
                ProviderName = relationship.ProviderName,
                EmployerAccountId = relationship.EmployerAccountId,
                LegalEntityId = relationship.LegalEntityId,
                LegalEntityName = relationship.LegalEntityName,
                LegalEntityAddress = relationship.LegalEntityAddress,
                LegalEntityOrganisationTypeId = (short)relationship.LegalEntityOrganisationType,
                LegalEntityOrganisationTypeDescription = relationship.LegalEntityOrganisationType.ToString(),
                Verified = relationship.Verified,
                IsLatest = true
            };

            await CommitmentsTestRepository.InsertIntoCommitmentsRelationships(relationshipsRecord);
        }
    }
}
