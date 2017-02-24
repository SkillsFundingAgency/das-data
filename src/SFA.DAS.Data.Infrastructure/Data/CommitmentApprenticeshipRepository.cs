using System.Data;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Domain.Models;

namespace SFA.DAS.Data.Infrastructure.Data
{
    public class CommitmentApprenticeshipRepository : BaseRepository, ICommitmentApprenticeshipRepository
    {
        public CommitmentApprenticeshipRepository(string connectionString) : base(connectionString)
        {

        }

        public async Task Create(CommitmentsApprenticeshipEvent @event)
        {
            //TODO: Get these from the database once these tables have been created
            var paymentStatusId = 0;
            var agreementStatusId = 0;

            await WithConnection(async c =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@commitmentId", @event.Id, DbType.Int64);
                parameters.Add("@paymentStatusId", paymentStatusId, DbType.Int32);
                parameters.Add("@apprenticeshipId", @event.ApprenticeshipId, DbType.Int64);
                parameters.Add("@agreementStatusId", agreementStatusId, DbType.Int32);
                parameters.Add("@ukPrn", @event.LearnerId, DbType.Int64);
                parameters.Add("@uln", @event.ProviderId, DbType.Int64);
                parameters.Add("@employerAccountId", @event.EmployerAccountId, DbType.String);
                parameters.Add("@trainingTypeId", @event.TrainingType, DbType.Int32);
                parameters.Add("@trainingId", @event.TrainingId, DbType.String);
                parameters.Add("@trainingStartDate", @event.TrainingStartDate, DbType.Date);
                parameters.Add("@trainingEndDate", @event.TrainingEndDate, DbType.Date);
                parameters.Add("@trainingTotalCost", @event.TrainingTotalCost, DbType.Decimal);

                return await c.ExecuteAsync(
                    sql: "[Data_Load].[CreateCommitmentApprenticeship]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure);
            });
        }
    }
}
