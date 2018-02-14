using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.Commitments.Api.Types;
using SFA.DAS.Data.Application.Interfaces.Repositories;

namespace SFA.DAS.Data.Infrastructure.Data
{
    public class RelationshipRepository : BaseRepository, IRelationshipRepository
    {
        public RelationshipRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task CreateRelationship(Relationship relationship)
        {
            await WithConnection(async c =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@employerAccountId", relationship.EmployerAccountId, DbType.Int64);
                parameters.Add("@legalEntityId", relationship.LegalEntityId, DbType.String);
                parameters.Add("@legalEntityName", relationship.LegalEntityName, DbType.String);
                parameters.Add("@legalEntityAddress", relationship.LegalEntityAddress, DbType.String);
                parameters.Add("@legalEntityOrganisationTypeId", relationship.LegalEntityOrganisationType, DbType.Byte);
                parameters.Add("@legalEntityOrganisationTypeDescription", relationship.LegalEntityOrganisationType.ToString(), DbType.String);
                parameters.Add("@providerId", relationship.ProviderId, DbType.Int64);
                parameters.Add("@providerName", relationship.ProviderName, DbType.String);
                parameters.Add("@verified", relationship.Verified, DbType.Boolean);

                return await c.ExecuteAsync(sql: "[Data_Load].[SaveRelationship]", param: parameters,
                    commandType: CommandType.StoredProcedure);
            });
        }

        public async Task VerifyRelationship(long providerId, long employerAccountId, string legalEntityId, bool? verified)
        {
            await WithConnection(async c =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("providerId", providerId, DbType.Int64);
                parameters.Add("@employerAccountId", employerAccountId, DbType.Int64);
                parameters.Add("@legalEntityId", legalEntityId, DbType.String);
                parameters.Add("@verified", verified, DbType.Boolean);

                return await c.ExecuteAsync(sql: "[Data_Load].[SaveRelationship]", param: parameters,
                    commandType: CommandType.StoredProcedure);
            });
        }
    }
}
