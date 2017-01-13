using System.Data;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.EAS.Account.Api.Client.Dtos;

namespace SFA.DAS.Data.Infrastructure.Data
{
    public class RegistrationRepository : BaseRepository, IRegistrationRepository 
    {
        public RegistrationRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task SaveRegistration(AccountInformationViewModel registration)
        {
            await WithConnection(async c =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@dasAccountName", registration.DasAccountName, DbType.String);
                parameters.Add("@dateRegistered", registration.DateRegistered, DbType.DateTime);
                parameters.Add("@legalEntityRegisteredAddress", registration.OrganisationRegisteredAddress, DbType.String);
                parameters.Add("@legalEntitySource", registration.OrganisationSource, DbType.String);
                parameters.Add("@legalEntityStatus", registration.OrganisationStatus, DbType.String);
                parameters.Add("@legalEntityName", registration.OrganisationName, DbType.String);
                parameters.Add("@legalEntityCreatedDate", registration.OrgansiationCreatedDate, DbType.DateTime);
                parameters.Add("@ownerEmail", registration.OwnerEmail, DbType.String);
                parameters.Add("@dasAccountId", registration.DasAccountId, DbType.String);
                parameters.Add("@legalEntityId", 0, DbType.Int32);
                parameters.Add("@legalEntityNumber", 0, DbType.Int32);

                return await c.ExecuteAsync(
                    sql: "INSERT INTO [Data_Load].[DAS_Employer_Registrations] ([DasAccountName],[DateRegistered],[LegalEntityRegisteredAddress],[LegalEntitySource],[LegalEntityStatus],[LegalEntityName], [LegalEntityCreatedDate], [OwnerEmail], [DasAccountId], [LegalEntityId], [LegalEntityNumber]) " +
                         "VALUES (@dasAccountName, @dateRegistered, @legalEntityRegisteredAddress, @legalEntitySource, @legalEntityStatus, @legalEntityName, @legalEntityCreatedDate, @ownerEmail, @dasAccountId, @legalEntityId, @legalEntityNumber)",
                    param: parameters,
                    commandType: CommandType.Text);
            });
        }
    }
}
