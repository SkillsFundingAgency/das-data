using System.Data;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.EAS.Account.Api.Types;

namespace SFA.DAS.Data.Infrastructure.Data
{
    public class EmployerAgreementRepository : BaseRepository, IEmployerAgreementRepository
    {
        public EmployerAgreementRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task SaveEmployerAgreement(EmployerAgreementView employerAgreement)
        {
            await WithConnection(async c =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@DasAccountId", employerAgreement.HashedAccountId, DbType.String);
                parameters.Add("@Status", employerAgreement.Status.ToString(), DbType.String);
                parameters.Add("@SignedBy", employerAgreement.SignedByName, DbType.String);
                parameters.Add("@SignedDate", employerAgreement.SignedDate, DbType.DateTime);
                parameters.Add("@ExpiredDate", employerAgreement.ExpiredDate, DbType.DateTime);
                parameters.Add("@DasLegalEntityId", employerAgreement.LegalEntityId, DbType.Int64);
                parameters.Add("@DasAgreementId", employerAgreement.HashedAgreementId, DbType.String);

                return await c.ExecuteAsync(
                    sql: "[Data_Load].[SaveEmployerAgreement]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure);
            });
        }
    }
}
