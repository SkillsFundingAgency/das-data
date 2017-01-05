using System.Data;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.Data.Application.Dtos;
using SFA.DAS.Data.Application.Interfaces;
using SFA.DAS.Data.Application.Interfaces.Repositories;

namespace SFA.DAS.Data.Infrastructure.Data
{
    public class RegistrationRepository : BaseRepository, IRegistrationRepository 
    {
        public async Task SaveRegistration(RegistrationViewModel registration)
        {
            await WithConnection(async c =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@dasAccountName", registration.DasAccountName, DbType.String);
                parameters.Add("@dasRegistered", registration.DateRegistered, DbType.DateTime);
                parameters.Add("@legalEntityRegisteredAddress", registration.OrganisationRegisteredAddress, DbType.String);
                parameters.Add("@legalEntitySource", registration.OrganisationSource, DbType.String);
                parameters.Add("@legalEntityStatus", registration.OrganisationStatus, DbType.String);
                parameters.Add("@legalEntityName", registration.OrganisationName, DbType.String);
                parameters.Add("@legalEntityCreatedDate", registration.OrgansiationCreatedDate, DbType.DateTime);
                parameters.Add("@ownerEmail", registration.OwnerEmail, DbType.String);
                parameters.Add("@dasAccountId", registration.DasAccoundId, DbType.String);
                parameters.Add("@legalEntityId", 0, DbType.Int32);
                parameters.Add("@companiesHouseNumber", 0, DbType.Int32);

                return await c.ExecuteAsync(
                    sql: "INSERT INTO [data].[Registration] ([DasAccountName],[DasRegistered],[LegalEntityRegisteredAddress],[LegalEntitySource],[LegalEntityStatus],[LegalEntityName], [LegalEntityCreatedDate], [OwnerEmail], [DasAccountId], [LegalEntityId], [CompaniesHouseNumber]) " +
                         "VALUES (@dasAccountName, @dasRegistered, @legalEntityRegisteredAddress, @legalEntitySource, @legalEntityStatus, @legalEntityName, @legalEntityCreatedDate, @ownerEmail, @dasAccountId, @legalEntityId, @companiesHouseNumber)",
                    param: parameters,
                    commandType: CommandType.Text);
            });
        }
    }
}
