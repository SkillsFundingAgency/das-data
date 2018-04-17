﻿using System.Data;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.Data.Application.Interfaces.Repositories;
using SFA.DAS.Data.Domain.Models;

namespace SFA.DAS.Data.Infrastructure.Data
{
    public class ApprenticeshipRepository : BaseRepository, IApprenticeshipRepository
    {
        public ApprenticeshipRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task Create(ApprenticeshipEvent @event)
        {
            await WithConnection(async c =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@commitmentId", @event.Id, DbType.Int64);
                parameters.Add("@paymentStatus", @event.PaymentStatus, DbType.String);
                parameters.Add("@apprenticeshipId", @event.ApprenticeshipId, DbType.Int64);
                parameters.Add("@agreementStatus", @event.AgreementStatus, DbType.String);
                parameters.Add("@ukPrn", @event.ProviderId, DbType.String);
                parameters.Add("@uln", @event.LearnerId, DbType.String);
                parameters.Add("@employerAccountId", @event.EmployerAccountId, DbType.String);
                parameters.Add("@trainingTypeId", @event.TrainingType, DbType.String);
                parameters.Add("@trainingId", @event.TrainingId, DbType.String);
                parameters.Add("@trainingStartDate", @event.TrainingStartDate, DbType.Date);
                parameters.Add("@trainingEndDate", @event.TrainingEndDate, DbType.Date);
                parameters.Add("@trainingTotalCost", @event.TrainingTotalCost, DbType.Decimal);
                parameters.Add("@legalEntityCode", @event.LegalEntityCode, DbType.String);
                parameters.Add("@legalEntityName", @event.LegalEntityName, DbType.String);
                parameters.Add("@legalEntityOrganisationType", @event.LegalEntityOrganisationType, DbType.String);
                parameters.Add("@dateOfBirth", @event.DateOfBirth, DbType.Date);
                parameters.Add("@transferSenderAccountId", @event.TransferSenderAccountId, DbType.Int64);
                parameters.Add("@transferApprovalStatus", @event.TransferApprovalStatus, DbType.String);
                parameters.Add("@transferApprovalDate", @event.TransferApprovalDate, DbType.DateTime);

                return await c.ExecuteAsync(
                    sql: "[Data_Load].[CreateCommitmentApprenticeship]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure);
            });
        }

        public async Task<long> GetTotalNumberOfAgreedApprenticeships()
        {
            return await WithConnection(async c =>
            {
                return await c.ExecuteScalarAsync<long>(
                    sql: "[PerformancePlatform].[GetNumberOfApprovedApprenticeships]",
                    commandType: CommandType.StoredProcedure);
            });
        }
    }
}
