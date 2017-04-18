using System.Data;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.EAS.Account.Api.Types;

namespace SFA.DAS.Data.Infrastructure.Data
{
    public class LevyDeclarationRepository : BaseRepository, ILevyDeclarationRepository
    {
        public LevyDeclarationRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task SaveLevyDeclaration(LevyDeclarationViewModel levyDeclaration)
        {
            await WithConnection(async c =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@DasAccountId", levyDeclaration.HashedAccountId, DbType.String);
                parameters.Add("@LevyDeclarationId", levyDeclaration.Id, DbType.Int64);
                parameters.Add("@PayeSchemeReference", levyDeclaration.PayeSchemeReference, DbType.String);
                parameters.Add("@LevyDueYearToDate", levyDeclaration.LevyDueYtd, DbType.Decimal);
                parameters.Add("@LevyAllowanceForYear", levyDeclaration.LevyAllowanceForYear, DbType.Decimal);
                parameters.Add("@SubmissionDate", levyDeclaration.SubmissionDate, DbType.DateTime);
                parameters.Add("@SubmissionId", levyDeclaration.SubmissionId, DbType.Int64);
                parameters.Add("@PayrollYear", levyDeclaration.PayrollYear, DbType.String);
                parameters.Add("@PayrollMonth", levyDeclaration.PayrollMonth, DbType.Int16);
                parameters.Add("@CreatedDate", levyDeclaration.CreatedDate, DbType.DateTime);
                parameters.Add("@EndOfYearAdjustment", levyDeclaration.EndOfYearAdjustment, DbType.Boolean);
                parameters.Add("@EndOfYearAdjustmentAmount", levyDeclaration.EndOfYearAdjustmentAmount, DbType.Decimal);
                parameters.Add("@DateCeased", levyDeclaration.DateCeased, DbType.DateTime);
                parameters.Add("@InactiveFrom", levyDeclaration.InactiveFrom, DbType.DateTime);
                parameters.Add("@InactiveTo", levyDeclaration.InactiveTo, DbType.DateTime);
                parameters.Add("@HmrcSubmissionId", levyDeclaration.HmrcSubmissionId, DbType.Int64);
                parameters.Add("@EnglishFraction", levyDeclaration.EnglishFraction, DbType.Decimal);
                parameters.Add("@TopupPercentage", levyDeclaration.TopUpPercentage, DbType.Decimal);
                parameters.Add("@TopupAmount", levyDeclaration.TopUp, DbType.Decimal);

                return await c.ExecuteAsync(
                    sql: "[Data_Load].[SaveLevyDeclaration]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure);
            });
        }
    }
}
