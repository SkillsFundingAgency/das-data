using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Domain.Models;

namespace SFA.DAS.Data.Infrastructure.Data
{
    public class StatisticsRepository : BaseRepository, IStatisticsRepository
    {
        public async Task<RdsStatisticsForEasModel> RetrieveEquivalentEasStatisticsFromRds()
        {

            var result = await WithConnection(async c => await c.QuerySingleOrDefaultAsync<RdsStatisticsForEasModel>(
                sql: "[Data_Load].[GetEasStatistics]",
                commandType: CommandType.StoredProcedure));

            return result;
        }

        public async Task SaveEasStatistics(EasStatisticsModel easStatisticsModel, RdsStatisticsForEasModel rdsStatisticsForEasModel)
        {
            await WithConnection(async c =>
            {
                using (var transaction = c.BeginTransaction())
                {
                    try
                    {
                        await SaveStatistic(c, transaction, nameof(easStatisticsModel.TotalPayments),
                            easStatisticsModel.TotalPayments,
                            rdsStatisticsForEasModel.TotalPayments);
                        await SaveStatistic(c, transaction, nameof(easStatisticsModel.TotalAccounts),
                            easStatisticsModel.TotalAccounts,
                            rdsStatisticsForEasModel.TotalAccounts);
                        await SaveStatistic(c, transaction, nameof(easStatisticsModel.TotalAgreements),
                            easStatisticsModel.TotalAgreements, rdsStatisticsForEasModel.TotalAgreements);
                        await SaveStatistic(c, transaction, nameof(easStatisticsModel.TotalLegalEntities),
                            easStatisticsModel.TotalLegalEntities, rdsStatisticsForEasModel.TotalLegalEntities);
                        await SaveStatistic(c, transaction, nameof(easStatisticsModel.TotalPAYESchemes),
                            easStatisticsModel.TotalPAYESchemes, rdsStatisticsForEasModel.TotalPAYESchemes);

                        transaction.Commit();
                    }
                    catch (SqlException e)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

                return 0;
            });
        }

        private static async Task SaveStatistic(IDbConnection c, IDbTransaction transaction, string dataType, int easValue, int rdsValue)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@dataType", dataType, DbType.String);
            parameters.Add("@checkedDateTime", DateTime.UtcNow, DbType.DateTime);
            parameters.Add("@sourceSystemCount", easValue, DbType.Int32);
            parameters.Add("@rdsCount", rdsValue, DbType.Int32);

            await c.ExecuteAsync(
                sql: "[Data_Load].[SaveConsistencyCheck]",
                param: parameters,
                commandType: CommandType.StoredProcedure,
                transaction: transaction);
        }

        public StatisticsRepository(string connectionString) : base(connectionString)
        {
        }
    }
}
