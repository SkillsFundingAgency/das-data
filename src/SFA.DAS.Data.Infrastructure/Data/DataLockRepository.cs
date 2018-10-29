using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Provider.Events.Api.Types;

namespace SFA.DAS.Data.Infrastructure.Data
{
    public class DataLockRepository : BaseRepository, IDataLockRepository
    {
        public DataLockRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task SaveDataLock(DataLockEvent dataLock)
        {
            await WithConnection(async c =>
            {
                using (var transaction = c.BeginTransaction())
                {
                    try
                    {
                        var parameters = new DynamicParameters();
                        parameters.Add("@DataLockId", dataLock.Id, DbType.Int64);
                        parameters.Add("@ProcessDateTime", dataLock.ProcessDateTime, DbType.DateTime);
                        parameters.Add("@IlrFileName", dataLock.IlrFileName, DbType.String);
                        parameters.Add("@UkPrn", dataLock.Ukprn, DbType.Int64);
                        parameters.Add("@Uln", dataLock.Uln, DbType.Int64);
                        parameters.Add("@LearnRefNumber", dataLock.LearnRefNumber, DbType.String);
                        parameters.Add("@AimSeqNumber", dataLock.AimSeqNumber, DbType.Int64);
                        parameters.Add("@PriceEpisodeIdentifier", dataLock.PriceEpisodeIdentifier, DbType.String);
                        parameters.Add("@ApprenticeshipId", dataLock.ApprenticeshipId, DbType.Int64);
                        parameters.Add("@EmployerAccountId", dataLock.EmployerAccountId, DbType.Int64);
                        parameters.Add("@EventSource", dataLock.EventSource, DbType.Int32);
                        parameters.Add("@Status", dataLock.Status, DbType.Int32);
                        parameters.Add("@HasErrors", dataLock.HasErrors, DbType.Boolean);
                        parameters.Add("@IlrStartDate", dataLock.IlrStartDate, DbType.Date);
                        parameters.Add("@IlrStandardCode", dataLock.IlrStandardCode, DbType.Int64);
                        parameters.Add("@IlrProgrammeType", dataLock.IlrProgrammeType, DbType.Int32);
                        parameters.Add("@IlrFrameworkCode", dataLock.IlrFrameworkCode, DbType.Int32);
                        parameters.Add("@IlrPathwayCode", dataLock.IlrPathwayCode, DbType.Int32);
                        parameters.Add("@IlrTrainingPrice", dataLock.IlrTrainingPrice, DbType.Decimal);
                        parameters.Add("@IlrEndpointAssessorPrice", dataLock.IlrEndpointAssessorPrice, DbType.Decimal);
                        parameters.Add("@IlrPriceEffectiveFromDate", dataLock.IlrPriceEffectiveFromDate, DbType.Date);
                        parameters.Add("@IlrPriceEffectiveToDate", dataLock.IlrPriceEffectiveToDate, DbType.Date);

                        var id = await c.ExecuteScalarAsync<long>(
                            sql: "[Data_Load].[SaveDataLock]",
                            param: parameters,
                            commandType: CommandType.StoredProcedure,
                            transaction: transaction);

                        if (dataLock.HasErrors)
                        {
                            await SaveDataLockErrors(c, transaction, id, dataLock.Errors);
                        }

                        await SaveDataLockPeriods(c, transaction, id, dataLock.Periods);
                        await SaveDataLockApprenticeships(c, transaction, id, dataLock.Apprenticeships);

                        transaction.Commit();
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

                return 0;
            });
        }

        private static async Task SaveDataLockApprenticeships(IDbConnection c, IDbTransaction transaction, long dataLockId, DataLockEventApprenticeship[] apprenticeships)
        {
            foreach (var apprenticeship in apprenticeships)
            {
                var parameters = new DynamicParameters();

                parameters.Add("@DataLockId", dataLockId, DbType.String);
                parameters.Add("@Version", apprenticeship.Version, DbType.String);
                parameters.Add("@StartDate", apprenticeship.StartDate, DbType.Date);
                parameters.Add("@StandardCode", apprenticeship.StandardCode, DbType.Int64);
                parameters.Add("@ProgrammeType", apprenticeship.ProgrammeType, DbType.Int32);
                parameters.Add("@FrameworkCode", apprenticeship.FrameworkCode, DbType.Int32);
                parameters.Add("@PathwayCode", apprenticeship.PathwayCode, DbType.Int32);
                parameters.Add("@NegotiatedPrice", apprenticeship.NegotiatedPrice, DbType.Decimal);
                parameters.Add("@EffectiveDate", apprenticeship.EffectiveDate, DbType.Date);

                await c.ExecuteAsync(
                    sql: "[Data_Load].[SaveDataLock_Apprenticeship]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure,
                    transaction: transaction);
            }
        }

        private static async Task SaveDataLockErrors(IDbConnection c, IDbTransaction transaction, long dataLockId, DataLockEventError[] errors)
        {
            foreach (var error in errors)
            {
                var parameters = new DynamicParameters();

                parameters.Add("@DataLockId", dataLockId, DbType.String);
                parameters.Add("@ErrorCode", error.ErrorCode, DbType.String);
                parameters.Add("@SystemDescription", error.SystemDescription, DbType.String);

                await c.ExecuteAsync(
                    sql: "[Data_Load].[SaveDataLock_Error]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure,
                    transaction: transaction);
            }
        }

        private static async Task SaveDataLockPeriods(IDbConnection c, IDbTransaction transaction, long dataLockId, DataLockEventPeriod[] periods)
        {
            foreach (var period in periods)
            {
                var parameters = new DynamicParameters();

                parameters.Add("@DataLockId", dataLockId, DbType.String);
                parameters.Add("@ApprenticeshipVersion", period.ApprenticeshipVersion, DbType.String);
                parameters.Add("@CollectionPeriodName", period.Period.Id, DbType.String);
                parameters.Add("@CollectionPeriodMonth", period.Period.Month, DbType.Int32);
                parameters.Add("@CollectionPeriodYear", period.Period.Year, DbType.Int32);
                parameters.Add("@IsPayable", period.IsPayable, DbType.Boolean);
                parameters.Add("@TransactionType", period.TransactionType, DbType.Int32);

                await c.ExecuteAsync(
                    sql: "[Data_Load].[SaveDataLock_Period]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure,
                    transaction: transaction);
            }
        }
    }
}
