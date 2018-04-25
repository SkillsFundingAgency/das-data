﻿using System;
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

        public async Task<RdsStatisticsForCommitmentsModel> RetrieveEquivalentCommitmentsStatisticsFromRds()
        {
            var result = await WithConnection(async c => await c.QuerySingleOrDefaultAsync<RdsStatisticsForCommitmentsModel>(
                sql: "[Data_Load].[GetCommitmentStatistics]",
                commandType: CommandType.StoredProcedure));

            return result;
        }

        public async Task<RdsStatisticsForPaymentsModel> RetrieveEquivalentPaymentStatisticsFromRds()
        {
            var result = await WithConnection(async c => await c.QuerySingleOrDefaultAsync<RdsStatisticsForPaymentsModel>(
                sql: "[Data_Load].[GetPaymentStatistics]",
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
                    catch (SqlException)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

                return 0;
            });
        }

        public async Task SaveCommitmentStatistics(CommitmentsStatisticsModel statisticsModel, RdsStatisticsForCommitmentsModel rdsModel)
        {
            await WithConnection(async c =>
            {
                using (var transaction = c.BeginTransaction())
                {
                    try
                    {
                        await SaveStatistic(c, transaction, nameof(statisticsModel.ActiveApprenticeships),
                            statisticsModel.ActiveApprenticeships,
                            rdsModel.ActiveApprenticeships);

                        await SaveStatistic(c, transaction, nameof(statisticsModel.TotalApprenticeships),
                            statisticsModel.TotalApprenticeships,
                            rdsModel.TotalApprenticeships);

                        await SaveStatistic(c, transaction, nameof(statisticsModel.TotalCohorts),
                            statisticsModel.TotalCohorts,
                            rdsModel.TotalCohorts);

                        transaction.Commit();
                    }
                    catch (SqlException)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

                return 0;
            });
        }

        public async Task SavePaymentStatistics(PaymentStatisticsModel statisticsModel, RdsStatisticsForPaymentsModel rdsModel)
        {
            await WithConnection(async c =>
            {
                using (var transaction = c.BeginTransaction())
                {
                    try
                    {
                        await SaveStatistic(c, transaction, nameof(statisticsModel.ProviderTotalPayments),
                            statisticsModel.ProviderTotalPayments,
                            rdsModel.ProviderTotalPayments);

                        transaction.Commit();
                    }
                    catch (SqlException)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

                return 0;
            });
        }

        private static async Task SaveStatistic(IDbConnection c, IDbTransaction transaction, string dataType, long easValue, long rdsValue)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@dataType", dataType, DbType.String);
            parameters.Add("@checkedDateTime", DateTime.UtcNow, DbType.DateTime);
            parameters.Add("@sourceSystemCount", easValue, DbType.Int64);
            parameters.Add("@rdsCount", rdsValue, DbType.Int64);

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